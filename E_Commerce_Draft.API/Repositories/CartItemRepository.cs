using E_Commerce_Draft.API.Models.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using static E_Commerce_Draft.API.Models.Domain.CartItem;
using static E_Commerce_Draft.API.Models.Domain.OrderDetail;

namespace E_Commerce_Draft.API.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly SqlConnection _connection;
        public CartItemRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("E_CommerceConnectionString"));
        }
        private CartItem MapCartItem(SqlDataReader reader)
        {
            return new CartItem
            {
                ID = (int)reader["CartItemId"], // CartItems.ID
                UserId = (int)reader["UserId"], // CartItems.UserID
                ProductId = (int)reader["ProductId"], // CartItems.ProductID
                Quantity = (int)reader["Quantity"], // CartItems.Quantity
                User = new User
                {
                    ID = (int)reader["UserId"], // Users.ID
                    Name = (string)reader["UserName"], // Users.Name
                    Email = (string)reader["UserEmail"]// Users.Email
                },
                Product = new Product
                {
                    ID = (int)reader["ProductId"], // Products.ID
                    Name = (string)reader["ProductName"], // Products.Name
                    Price = (decimal)reader["ProductPrice"], // Products.Price
                    CategoryId = (int)reader["ProductCategoryId"], // Products.CategoryID
                    Categories = reader["ProductCategoryName"] == DBNull.Value ? null : new Category
                    {
                        Name = (string)reader["ProductCategoryName"]
                    }
                }
            };
        }

        public async Task<CartItemResponseModel> CreateCartItemAsync(CartItem cartItem)
        {
            var cartItemCreateResponseModel = new CartItemResponseModel
            {
                CartItem = null,
                MessageId = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (SqlCommand command = new SqlCommand("usp_CreateCartItem", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@UserId", cartItem.UserId));
                    command.Parameters.Add(new SqlParameter("@ProductId", cartItem.ProductId));
                    command.Parameters.Add(new SqlParameter("@Quantity", cartItem.Quantity));

                    // Add output parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            cartItemCreateResponseModel.CartItem = MapCartItem(reader);
                        }
                    }
                    cartItemCreateResponseModel.MessageId = (int)messageIdParam.Value;
                    cartItemCreateResponseModel.MessageDescription = (string)messageDescriptionParam.Value;
                }
            }

            catch (SqlException sqlEx)
            {
                cartItemCreateResponseModel.MessageId = -99;
                cartItemCreateResponseModel.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                cartItemCreateResponseModel.MessageId = -100;
                cartItemCreateResponseModel.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }

            return cartItemCreateResponseModel;


        }

        public async Task<CartItemResponseModel> DeleteCartItemAsync(int cartItemId)
        {
            var responseModel = new CartItemResponseModel
            {
                CartItem = null,
                MessageId = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using (SqlCommand command = new SqlCommand("usp_DeleteCartItem", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CartItemID", cartItemId);

                    // Add output parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

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

        public async Task<GetAllCartItemsResponseModel> GetAllCartItemsAsync()
        {
            var getAllCartItemsResponseModel = new GetAllCartItemsResponseModel
            {
                CartItems = new List<CartItem>(),
                MessageId = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetAllCartItems", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add output parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    List<CartItem> cartItems = new List<CartItem>();

                    await _connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            getAllCartItemsResponseModel.CartItems.Add(MapCartItem(reader));
                        }
                    }
                    
                    getAllCartItemsResponseModel.MessageId = (int)messageIdParam.Value;
                    getAllCartItemsResponseModel.MessageDescription = (string)messageDescriptionParam.Value;

                }
            }
            catch (SqlException sqlEx)
            {
                getAllCartItemsResponseModel.MessageId = -99;
                getAllCartItemsResponseModel.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                getAllCartItemsResponseModel.MessageId = -100;
                getAllCartItemsResponseModel.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }

            return getAllCartItemsResponseModel;
        }

        public async Task<GetCartItemsByUserIdResponseModel> GetCartItemsByUserIdAsync(int userId)
        {
            var getCartItemsByUserIdResponseModel = new GetCartItemsByUserIdResponseModel
            {
                CartItems = new List<CartItem>(),
                MessageId = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetCartItemsByUserId", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);

                    // Add output parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            getCartItemsByUserIdResponseModel.CartItems.Add(MapCartItem(reader));
                        }
                    }
                    getCartItemsByUserIdResponseModel.MessageId = (int)messageIdParam.Value;
                    getCartItemsByUserIdResponseModel.MessageDescription = (string)messageDescriptionParam.Value;
                }
            }
            catch (SqlException sqlEx)
            {
                getCartItemsByUserIdResponseModel.MessageId = -99;
                getCartItemsByUserIdResponseModel.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                getCartItemsByUserIdResponseModel.MessageId = -100;
                getCartItemsByUserIdResponseModel.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
            return getCartItemsByUserIdResponseModel;
        }

        public async Task<CartItemResponseModel> UpdateCartItemAsync(CartItem cartItem)
        {
            var cartItemResponseModel = new CartItemResponseModel
            {
                CartItem = null,
                MessageId = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateCartItem", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CartItemID", cartItem.ID);
                    command.Parameters.AddWithValue("@Quantity", cartItem.Quantity);

                    // Add output parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    CartItem? updatedCartItem = null;
                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            cartItemResponseModel.CartItem = MapCartItem(reader);
                        }
                    }

                    cartItemResponseModel.MessageId = (int)messageIdParam.Value;
                    cartItemResponseModel.MessageDescription = (string)messageDescriptionParam.Value;
                }
            }
            catch (SqlException sqlEx)
            {
                cartItemResponseModel.MessageId = -99;
                cartItemResponseModel.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                cartItemResponseModel.MessageId = -100;
                cartItemResponseModel.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
            return cartItemResponseModel;
        }
    }
}