using E_Commerce_Draft.API.Models.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using static E_Commerce_Draft.API.Models.Domain.Order;
using static E_Commerce_Draft.API.Models.Domain.OrderDetail;

namespace E_Commerce_Draft.API.Repositories
{
    public class OrderDetailRepository : IOrderDetailsRepository
    {
        private readonly SqlConnection _connection;

        public OrderDetailRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("E_CommerceConnectionString"));
        }

        private OrderDetail MapOrderDetail(SqlDataReader reader)
        {
            return new OrderDetail
            {
                ID = (int)(reader["OrderDetailId"]),
                OrderID = Convert.ToInt32(reader["OrderId"]),
                ProductID = Convert.ToInt32(reader["ProductId"]),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                Product = new Product
                {
                    ID = (int)reader["ProductId"],
                    Name = (string)reader["ProductName"],
                    Price = (Decimal)reader["ProductPrice"]
                },
                Order = new Order
                {
                    ID = (int)reader["OrderId"],
                    UserID = (int)reader["UserId"],
                    OrderDate = (DateTime)reader["OrderDate"],
                    TotalPrice = (Decimal)reader["OrderTotalPrice"],
                    isDeleted = (bool)reader["isDeleted"]
                }
            };
        }


        public async Task<OrderDetailResponseModel> CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            var orderResponseModel = new OrderDetailResponseModel
            {
                OrderDetail = null,
                MessageId = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using (SqlCommand command = new SqlCommand("usp_CreateOrderDetail", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ProductId", orderDetail.ProductID);
                    command.Parameters.AddWithValue("@OrderId", orderDetail.OrderID);
                    command.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);

                    //Output Parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            orderResponseModel.OrderDetail = MapOrderDetail(reader);
                        }
                    }

                    orderResponseModel.MessageId = (int)messageIdParam.Value;
                    orderResponseModel.MessageDescription = (string)messageDescriptionParam.Value;
                }
            }
            catch (SqlException sqlEx)
            {
                orderResponseModel.MessageId = -99;
                orderResponseModel.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                orderResponseModel.MessageId = -100;
                orderResponseModel.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }

            return orderResponseModel;

        }

        public async Task<bool> DeleteOrderDetailAsync(int orderId, int productId)
        {
            bool isDeleted = false;
            try
            {
                using (SqlCommand command = new SqlCommand("usp_DeleteOrderDetail", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.Parameters.AddWithValue("@ProductId", productId);

                    //Output Parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    var messageId = (int)messageIdParam.Value;
                    var messageDescription = (string)messageDescriptionParam.Value;

                    if (messageId == 1)
                    {
                        isDeleted = true; // Başarılı silme işlemi
                        Console.WriteLine(messageDescription);
                    }
                    else
                    {
                        Console.WriteLine($"Error: {messageDescription}");
                    }


                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }

            return isDeleted;

        }

        public async Task<GetAllOrderDetailsResponseModel> GetAllOrderDetailsAsync()
        {
            var responseModel = new GetAllOrderDetailsResponseModel
            {
                OrderDetails = new List<OrderDetail>(),
                MessageId = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetAllOrderDetails", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            responseModel.OrderDetails.Add(MapOrderDetail(reader));
                        }
                    }

                    responseModel.MessageId = (int)messageIdParam.Value;
                    responseModel.MessageDescription = (string)messageDescriptionParam.Value;

                }
            }
            catch (SqlException sqlEx)
            {
                responseModel.MessageId = -99;
                responseModel.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                responseModel.MessageId = -100;
                responseModel.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }

            return responseModel;
        }

        public async Task<GetAllOrderDetailsResponseModel> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var orderDetailResponseModel = new GetAllOrderDetailsResponseModel
            {
                OrderDetails = new List<OrderDetail>(),
                MessageId = 0,
                MessageDescription = string.Empty
            };


            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetOrderDetailsByOrderId", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    //Output Parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        { 
                            orderDetailResponseModel.OrderDetails.Add(MapOrderDetail(reader));
                        }
                    }


                    orderDetailResponseModel.MessageId = (int)messageIdParam.Value;
                    orderDetailResponseModel.MessageDescription = (string)messageDescriptionParam.Value;
                }
            }

            catch (SqlException sqlEx)
            {
                orderDetailResponseModel.MessageId = -99;
                orderDetailResponseModel.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                orderDetailResponseModel.MessageId = -100;
                orderDetailResponseModel.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }

            return orderDetailResponseModel;
        }

        public async Task<OrderDetailResponseModel> UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            var responseModel = new OrderDetailResponseModel();

            try
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateOrderDetail", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", orderDetail.OrderID);
                    command.Parameters.AddWithValue("@ProductId", orderDetail.ProductID);
                    command.Parameters.AddWithValue("@NewQuantity", orderDetail.Quantity);

                    //Output Parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();

                    responseModel.MessageId = (int)messageIdParam.Value;
                    responseModel.MessageDescription = (string)messageDescriptionParam.Value;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            responseModel.OrderDetail = MapOrderDetail(reader);
                        }
                    }

                    responseModel.MessageId = (int)messageIdParam.Value;
                    responseModel.MessageDescription = (string)messageDescriptionParam.Value;


                }


            }
            catch (SqlException sqlEx)
            {
                responseModel.MessageId = -99;
                responseModel.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                responseModel.MessageId = -100;
                responseModel.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }

            return responseModel;

        }
    }
}
