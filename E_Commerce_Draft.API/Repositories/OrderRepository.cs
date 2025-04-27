using E_Commerce_Draft.API.Models.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using static E_Commerce_Draft.API.Models.Domain.Order;

namespace E_Commerce_Draft.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SqlConnection _connection;
        public OrderRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("E_CommerceConnectionString"));
        }

        private Order MapOrder(SqlDataReader reader)
        {
            return new Order
            {
                ID = (int)reader["OrderId"],            
                UserID = (int)reader["UserId"],         
                OrderDate = (DateTime)reader["OrderDate"], 
                TotalPrice = (decimal)reader["TotalPrice"], 
                isDeleted = (bool)reader["OrderDeleted"], 
                Users = new User
                {
                    ID = (int)reader["UserId"],          
                    Name = (string)reader["UserName"],   
                    Email = (string)reader["UserEmail"],
                    PasswordHash = (string)reader["UserPasswordHash"],
                    isDeleted = (bool)reader["UserDeleted"]
                }
            };
        }
        public async Task<OrderResponseModel> CreateOrderAsync(Order order)
        {
            OrderResponseModel orderResponseModel = new OrderResponseModel();

            try
            {
                using (SqlCommand command = new SqlCommand("[usp_CreateOrder]", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", order.UserID);
                    command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    command.Parameters.AddWithValue("@TotalPrice", order.TotalPrice);

                    //Output Parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    Order? createdOrder = null;

                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if(await reader.ReadAsync())
                        {
                            createdOrder = MapOrder(reader);
                        }
                    }
                    orderResponseModel.Order = createdOrder;
                    orderResponseModel.MessageId = (int)messageIdParam.Value;
                    orderResponseModel.MessageDescription = (string)messageDescriptionParam.Value;
                    return orderResponseModel;
                    //return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, createdOrder);
                }
            }
            catch (SqlException sqlEx)
            {
                orderResponseModel.MessageId = -99;
                orderResponseModel.MessageDescription = $"Database error: {sqlEx.Message}";
                orderResponseModel.Order = null;
                return orderResponseModel;
            }
            catch (Exception ex)
            {
                orderResponseModel.MessageId = -100;
                orderResponseModel.MessageDescription = $"Unexpected error: {ex.Message}";
                orderResponseModel.Order = null;
                return orderResponseModel;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
        }

        public async Task<OrderListResponseModel> GetAllOrdersAsync()
        {
            var orderListResponseModel = new OrderListResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Orders = new List<Order>()
            };
            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetAllOrders", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Output Parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orderListResponseModel.Orders.Add(MapOrder(reader));
                        }
                    }
                    orderListResponseModel.MessageId = (int)messageIdParam.Value;
                    orderListResponseModel.MessageDescription = (string)messageDescriptionParam.Value;
                }
            }
            catch (SqlException sqlEx)
            {
                orderListResponseModel.MessageId = -99;
                orderListResponseModel.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                orderListResponseModel.MessageId = -100;
                orderListResponseModel.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();

            }
            return orderListResponseModel;
        }

        public async Task<OrderResponseModel> GetOrderByIdAsync(int orderId)
        {
            var orderResponseModel = new OrderResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Order = null
            };
            try
            {
                using (SqlCommand command = new SqlCommand("[usp_GetOrderById]", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", orderId);

                    // Output Parameters

                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    Order? order = null;

                    await _connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            orderResponseModel.Order = MapOrder(reader);
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

        public async Task<OrderResponseModel> UpdateOrderAsync(Order order)
        {
            var orderResponseModel = new OrderResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Order = null
            };
            try
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateOrder", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@OrderID", order.ID);
                    command.Parameters.AddWithValue("@UserID", order.UserID);
                    command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    command.Parameters.AddWithValue("@TotalPrice", order.TotalPrice);
                    command.Parameters.AddWithValue("@IsDeleted", order.isDeleted);

                    // Output Parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    Order? updatedOrder = null;

                    await _connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            orderResponseModel.Order = MapOrder(reader);
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
    }
}
