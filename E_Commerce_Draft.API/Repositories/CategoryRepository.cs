using E_Commerce_Draft.API.Models.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
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

        private Category MapCategoryResponse(SqlDataReader reader)
        {
            return new Category
            {
                ID = (int)reader["CategoryId"],
                Name = (string)reader["CategoryName"],
                isDeleted = (bool)reader["isDeleted"]  
            };
        }


        public async Task<CategoryResponseModel> CreateCategoryAsync(Category category)
        {
            var responseModel = new CategoryResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Category = null
            };
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

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            responseModel.Category = MapCategoryResponse(reader);
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

        public async Task<CategoryListResponseModel> GetAllCategoriesAsync()
        {
            var responseModel = new CategoryListResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Categories = new List<Category>()
            };
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
                            responseModel.Categories.Add(MapCategoryResponse(reader));
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

        public async Task<CategoryResponseModel> GetCategoryByIdAsync(int categoryId)
        {
            var responseModel = new CategoryResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Category = null
            };
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
                            responseModel.Category = MapCategoryResponse(reader);
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

        public async Task<CategoryResponseModel> UpdateCategoryAsync(Category category)
        {
            var responseModel = new CategoryResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Category = null
            };
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
                            responseModel.Category = MapCategoryResponse(reader);
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
