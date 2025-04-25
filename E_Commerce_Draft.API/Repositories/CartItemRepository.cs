using E_Commerce_Draft.API.Models.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

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
                ID = (int)(reader["Id"]),
                UserId = (int)reader["UserId"],
                ProductId = (int)(reader["ProductId"]),
                Quantity = (int)(reader["Quantity"]),
                User = new User
                {
                    ID = (int)(reader["UserId"]),
                    Name = (string)reader["UserName"]
                },
                Product = new Product
                {
                    ID = (int)reader["ProductId"],
                    Name = (string)reader["ProductName"],
                    Price = (Decimal)reader["ProductPrice"]
                }
            };
        }
        public async Task<(int MessageId, string MessageDescription, CartItem?)> CreateCartItemAsync(CartItem cartItem)
        {
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

                    CartItem? newCartItem = null;
                    await _connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            newCartItem = MapCartItem(reader);
                        }

                    }
                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, newCartItem);
                }
            }

            catch (SqlException sqlEx)
            {
                return (-99, $"Database error: {sqlEx.Message}", null);
            }
            catch (Exception ex)
            {
                return (-100, $"Unexpected error: {ex.Message}", null);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }

        }

        public async Task<(int MessageId, string MessageDescription)> DeleteCartItemAsync(int cartItemId)
        {
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

                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value);
                }
            }

            catch (SqlException sqlEx)
            {
                return (-99, $"Database error: {sqlEx.Message}"); // SQL hataları için
            }
            catch (Exception ex)
            {
                return (-100, $"Unexpected error: {ex.Message}"); // Genel hatalar için
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync(); // Bağlantıyı kapat
            }
        }

        public async Task<(int MessageId, string MessageDescription, List<CartItem>?)> GetAllCartItemsAsync()
        {
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
                            CartItem cartItem = MapCartItem(reader);
                            cartItems.Add(cartItem);
                        }
                    }

                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, cartItems);
                }
            }
            catch (SqlException sqlEx)
            {
                return (-99, $"Database error: {sqlEx.Message}", null);
            }
            catch (Exception ex)
            {
                return (-100, $"Unexpected error: {ex.Message}", null);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
        }

        public async Task<(int MessageId, string MessageDescription, List<CartItem>?)> GetCartItemsByUserIdAsync(int userId)

        {
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

                    List<CartItem> cartItems = new List<CartItem>();
                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            CartItem cartItem = MapCartItem(reader);
                            cartItems.Add(cartItem);
                        }
                    }

                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, cartItems);
                }
            }
            catch (SqlException sqlEx)
            {
                return (-99, $"Database error: {sqlEx.Message}", null);
            }
            catch (Exception ex)
            {
                return (-100, $"Unexpected error: {ex.Message}", null);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
        }

        public async Task<(int MessageId, string MessageDescription, CartItem?)> UpdateCartItemAsync(CartItem cartItem)
        {
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
                            updatedCartItem = MapCartItem(reader);
                        }
                    }
                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, updatedCartItem);
                }
            }
            catch (SqlException sqlEx)
            {
                return (-99, $"Database error: {sqlEx.Message}", null);
            }
            catch (Exception ex)
            {
                return (-100, $"Unexpected error: {ex.Message}", null);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
        }
    }
}