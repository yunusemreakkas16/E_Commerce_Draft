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
        public async Task<Category?> CreateCategoryAsync(Category category)
        {
            try 
            {
                using (SqlCommand command = new SqlCommand("usp_CreateCategory", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Name", category.Name));

                    await _connection.OpenAsync();
                    var result = await command.ExecuteNonQueryAsync();

                    if (result > 0)
                    {
                        return category;
                    }
                    
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something has happaned. " + ex.Message);
                return null;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = new List<Category>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetAllCategories", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await _connection.OpenAsync();
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something has happaned. " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(CategoryDetailParamModel categoryDetailParamModel)
        {
            Category? category = null;

            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetCategoryById", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CategoryId", categoryDetailParamModel.ID));
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
                }
                await _connection.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something has happaned. " + ex.Message);
            }

            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(Category category)
        {
            try {
                using (SqlCommand command = new SqlCommand("usp_UpdateCategory", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CategoryId", category.ID));
                    command.Parameters.Add(new SqlParameter("@Name", category.Name));
                    command.Parameters.Add(new SqlParameter("@isDeleted", category.isDeleted));

                    await _connection.OpenAsync();
                    var result = await command.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        return category;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something has happaned. " + ex.Message);
                return null;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
    }
}
