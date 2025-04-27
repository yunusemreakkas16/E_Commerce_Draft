using E_Commerce_Draft.API.Models.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using static E_Commerce_Draft.API.Models.Domain.Product;

namespace E_Commerce_Draft.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlConnection _connection;
        public ProductRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("E_CommerceConnectionString"));
        }

        private Product MapProduct(SqlDataReader reader)
        {
            return new Product
            {
                ID = (int)reader["ProductId"],
                Name = (string)reader["ProductName"],
                Price = (decimal)reader["ProductPrice"],
                CategoryId = (int)reader["ProductCategoryId"],
                isDeleted = (bool)reader["ProductDeleted"],
                Categories = new Category
                {
                    ID = (int)reader["CategoryId"],
                    Name = (string)reader["CategoryName"],
                    isDeleted = (bool)reader["CategoryDeleted"]
                }
            };
        }

        public async Task<ProductResponseModel> CreateProductAsync(Product product)
        {
            var responseModel = new ProductResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Product = null
            };

            try
            {
                using (SqlCommand command = new SqlCommand("usp_CreateProduct", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Name", product.Name));
                    command.Parameters.Add(new SqlParameter("@Price", product.Price));
                    command.Parameters.Add(new SqlParameter("@CategoryId", product.CategoryId));

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
                            responseModel.Product = MapProduct(reader);
                        }
                        else
                        {
                            responseModel.MessageId = -1;
                            responseModel.MessageDescription = "No product data returned.";
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

        public async Task<GetAllProductsResponseModel> GetAllProductsAsync()
        {
            var responseModel = new GetAllProductsResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Products = new List<Product>()
            };

            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetAllProducts", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

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
                            responseModel.Products.Add(MapProduct(reader));
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

        public async Task<ProductResponseModel> GetProductByIdAsync(int productId)
        {
            var responseModel = new ProductResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Product = null
            };
            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetProductById", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ProductId", productId));

                    // Output Parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    await _connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            responseModel.Product = MapProduct(reader);
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

        public async Task<ProductResponseModel> UpdateProductAsync(Product product)
        {
            var responseModel = new ProductResponseModel()
            {
                MessageId = 0,
                MessageDescription = string.Empty,
                Product = null
            };
            try
            {
                using (SqlCommand command = new SqlCommand("[usp_UpdateProduct]", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ProductId", product.ID));
                    command.Parameters.Add(new SqlParameter("@Name", product.Name));
                    command.Parameters.Add(new SqlParameter("@Price", product.Price));
                    command.Parameters.Add(new SqlParameter("@CategoryId", product.CategoryId));
                    command.Parameters.Add(new SqlParameter("@isDeleted", product.isDeleted));

                    //Output Parameters
                    var messageIdParam = new SqlParameter("@MessageId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageDescriptionParam = new SqlParameter("@MessageDescription", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(messageIdParam);
                    command.Parameters.Add(messageDescriptionParam);

                    Product? updatedProduct = null;

                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            responseModel.Product = MapProduct(reader);
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
