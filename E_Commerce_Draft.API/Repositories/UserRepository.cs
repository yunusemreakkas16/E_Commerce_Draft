using E_Commerce_Draft.API.Models.Domain;
using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
using static E_Commerce_Draft.API.Models.Domain.User;

namespace E_Commerce_Draft.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _connection;

        public UserRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("E_CommerceConnectionString"));
        }

        private User MapUser(SqlDataReader reader)
        {
            return new User
            {
                ID = (int)reader["UserId"],  
                Name = (string)reader["UserName"],
                Email = (string)reader["UserEmail"], 
                PasswordHash = (string)reader["PasswordHash"],
                isDeleted = (bool)reader["UserDeleted"]
            };
        }



        public async Task<UserResponseModel> CreateUserAsync(User user)
        {
            var userResponse = new UserResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                User = null
            };

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
                        userResponse.User = MapUser(reader);
                    }
                }

                userResponse.MessageId = (int)messageIdParam.Value;
                userResponse.MessageDescription = (string)messageDescriptionParam.Value;
            }
            catch (SqlException sqlEx)
            {
                userResponse.MessageId = -99;
                userResponse.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                userResponse.MessageId = -100;
                userResponse.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
            return userResponse;
        }

        public async Task<UserListResponseModel> GetAllUsersAsync()
        {
            var userListResponse = new UserListResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Users = new List<User>()
            };
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
                            userListResponse.Users.Add(MapUser(reader));
                        }
                    }
                    userListResponse.MessageId = (int)messageIdParam.Value;
                    userListResponse.MessageDescription = (string)messageDescriptionParam.Value;
                }
            }
            catch (SqlException sqlEx)
            {
                userListResponse.MessageId = -99;
                userListResponse.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                userListResponse.MessageId = -100;
                userListResponse.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
            return userListResponse;

        }

        public async Task<UserResponseModel> GetUserByIdAsync(int userId)
        {
            var userResponse = new UserResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                User = null
            };

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

                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            userResponse.User = MapUser(reader);
                        }
                    }

                    userResponse.MessageId = (int)messageIdParam.Value;
                    userResponse.MessageDescription = (string)messageDescriptionParam.Value;
                }
            }
            catch (SqlException sqlEx)
            {
                userResponse.MessageId = -99;
                userResponse.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                userResponse.MessageId = -100;
                userResponse.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
            return userResponse;
        }

        public async Task<UserResponseModel> UpdateUserAsync(User user)
        {
            var userResponseModel = new UserResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                User = null
            };
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
                            userResponseModel.User = MapUser(reader);
                        }
                    }
                    userResponseModel.MessageId = (int)messageIdParam.Value;
                    userResponseModel.MessageDescription = (string)messageDescriptionParam.Value;
                }
            }

            catch (SqlException sqlEx)
            {
                userResponseModel.MessageId = -99;
                userResponseModel.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                userResponseModel.MessageId = -100;
                userResponseModel.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }
            return userResponseModel;
        }

    }
}
