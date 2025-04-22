using E_Commerce_Draft.API.Models.Domain;
using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace E_Commerce_Draft.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _connection;

        public UserRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("E_CommerceConnectionString"));
        }
        public async Task<(int MessageId, string MessageDescription, User?)> CreateUserAsync(User user)

        {
            try 
            {
                SqlCommand command = new SqlCommand("usp_CreateUser", _connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Name", user.Name));
                command.Parameters.Add(new SqlParameter("@Email", user.Email));
                command.Parameters.Add(new SqlParameter("@PasswordHash", user.PasswordHash));

                // Output parameters
                var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                command.Parameters.Add(messageIdParam);
                command.Parameters.Add(messageDescriptionParam);

                User? User = null;

                await _connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        User = new User
                        {
                            ID = (int)reader["UserID"],
                            Name = (string)reader["UserName"],
                            Email = (string)reader["Email"],
                        };
                    }
                }
                int messageId = (int)messageIdParam.Value;
                string messageDescription = (string)messageDescriptionParam.Value;
                return (messageId, messageDescription, User);
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

        public async Task<(int MessageId, string MessageDescription, List<User>)> GetAllUsersAsync()
        {
            var users = new List<User>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetAllUsers", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Output parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new User
                            {
                                ID = (int)reader["UserID"],
                                Name = (string)reader["UserName"],
                                Email = (string)reader["Email"],
                                isDeleted = (bool)reader["isDeleted"]
                            });
                        }
                    }
                    int messageId = (int)messageIdParam.Value;
                    string messageDescription = (string)messageDescriptionParam.Value;
                    return (messageId, messageDescription, users);
                }
            }
            catch (SqlException sqlEx)
            {
                return (-99, $"Database error: {sqlEx.Message}", new List<User>());
            }
            catch (Exception ex)
            {
                return (-100, $"Unexpected error: {ex.Message}", new List<User>());
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }

        }

        public async Task<(int MessageId, string MessageDescription, User?)> GetUserByIdAsync(int userId)

        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetUserById", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@UserId", userId));

                    // Output parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    User? userDetail = null;


                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            userDetail = new User
                            {
                                ID = (int)reader["UserID"],
                                Name = (string)reader["UserName"],
                                Email = (string)reader["Email"],
                                isDeleted = (bool)reader["isDeleted"]
                            };
                        }
                    }
                    int messageId = (int)messageIdParam.Value;
                    string messageDescription = (string)messageDescriptionParam.Value;
                    return (messageId, messageDescription, userDetail);
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

        public async Task<(int MessageId, string MessageDescription, User?)> UpdateUserAsync(User user)
        {
            try
            {
                await _connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("usp_UpdateUser", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", user.ID);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@IsDeleted", user.isDeleted);

                    // Output parametreler
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    User? updatedUser = null;
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            updatedUser = new User
                            {
                                        ID = (int)(reader["UserID"]),
                                        Name = (string)reader["UserName"],
                                        Email = (string)reader["Email"],
                                        isDeleted = (bool)reader["isDeleted"]
                            };
                        }
                    }
                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, updatedUser);
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
