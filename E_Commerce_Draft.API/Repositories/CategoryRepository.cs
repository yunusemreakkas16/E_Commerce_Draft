using E_Commerce_Draft.API.Models.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace E_Commerce_Draft.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SqlConnection _connection;

        
        public CategoryRepository(IConfiguration configuration)
        {
           _connection = new SqlConnection(configuration.GetConnectionString("E_CommerceConnectionString"));
        }
        public async Task<(int MessageId, string MessageDescription, Category?)> CreateCategoryAsync(Category category)

        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_CreateCategory", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Name", category.Name));

                    // Add output parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);


                    Category? newCategory = null;

                    await _connection.OpenAsync();
                    var result = await command.ExecuteNonQueryAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            newCategory = new Category
                            {
                                ID = (int)reader["ID"],
                                Name = (string)reader["Name"]
                            };
                        }
                    }

                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, newCategory);
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

        public async Task<(int MessageId, string MessageDescription, List<Category>)> GetAllCategoriesAsync()

        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetAllCategories", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add output parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();

                    List<Category> categories = new List<Category>();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            categories.Add(new Category
                            {
                                ID = (int)reader["ID"],
                                Name = (string)reader["Name"],
                                isDeleted = (bool)reader["isDeleted"]
                            });
                        }
                    }
                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, categories);
                }                
            }
            catch (SqlException sqlEx)
            {
                return (-99, $"Database error: {sqlEx.Message}", new List<Category>());
            }
            catch (Exception ex)
            {
                return (-100, $"Unexpected error: {ex.Message}", new List<Category>());
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }

        }

        public async Task<(int MessageId, string MessageDescription, Category?)> GetCategoryByIdAsync(int categoryId)

        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetCategoryById", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CategoryId", categoryId));

                    // Add output parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    Category? category = null;
                    await command.Connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            category = new Category
                            {
                                ID = (int)(reader["Id"]),
                                Name = (string)reader["Name"],
                                isDeleted = (bool)reader["isDeleted"]
                            };
                        }
                    }
                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, category);
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


        public async Task<(int MessageId, string MessageDescription, Category?)> UpdateCategoryAsync(Category category)
        {
            try
            { 
                using (SqlCommand command = new SqlCommand("usp_UpdateCategory", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CategoryId", category.ID));
                    command.Parameters.Add(new SqlParameter("@Name", category.Name));
                    command.Parameters.Add(new SqlParameter("@isDeleted", category.isDeleted));

                    // Add output parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    Category? updatedCategory = null;

                    await command.Connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            updatedCategory = new Category
                            {
                                ID = (int)reader["ID"],
                                Name = (string)reader["Name"],
                                isDeleted = (bool)(reader["isDeleted"])
                            };
                        }
                    }
                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, updatedCategory);
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
