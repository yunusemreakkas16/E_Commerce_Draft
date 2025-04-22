using E_Commerce_Draft.API.Models.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace E_Commerce_Draft.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlConnection _connection;
        public ProductRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("E_CommerceConnectionString"));
        }
        public async Task<(int MessageId, string MessageDescription, Product?)> CreateProductAsync(Product product)
        {
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

                    Product? createdProduct = null;

                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            createdProduct = new Product
                            {
                                ID = (int)reader["ID"],
                                Name = (string)reader["Name"],
                                Price = (decimal)reader["Price"],
                                CategoryId = (int)reader["CategoryId"],
                                isDeleted = (bool)reader["isDeleted"],
                                Categories = new Category
                                {
                                    ID = (int)(reader["CategoryID"]),
                                    Name = (string)reader["CategoryName"],
                                    isDeleted = (bool)(reader["CategoryDeleted"])
                                }

                            };
                        }
                    }
                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, createdProduct);

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

        public async Task<(int MessageId, string MessageDescription, List<Product>)> GetAllProductsAsync()

        {
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

                    List<Product> products = new List<Product>();


                    await _connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            products.Add(new Product
                            {
                                ID = (int)reader["ProductID"],
                                Name = (string)reader["ProductName"],
                                Price = (decimal)reader["Price"],
                                CategoryId = (int)reader["CategoryId"],
                                isDeleted = (bool)reader["isDeleted"],
                                Categories = new Category
                                {
                                    ID = (int)(reader["CategoryID"]),
                                    Name = (string)reader["CategoryName"],
                                    isDeleted = (bool)(reader["CategoryDeleted"])
                                }
                            });
                        }
                    }
                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, products);
                }
            }
            catch (SqlException sqlEx)
            {
                return (-99, $"Database error: {sqlEx.Message}", new List<Product>());
            }
            catch (Exception ex)
            {
                return (-100, $"Unexpected error: {ex.Message}", new List<Product>());
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    await _connection.CloseAsync();
            }

        }

        public async Task<(int MessageId, string MessageDescription, Product?)> GetProductByIdAsync(int productId)

        {
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

                    Product? product = null;


                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            product = new Product
                            {
                                ID = (int)reader["ProductID"],
                                Name = (string)reader["ProductName"],
                                Price = (decimal)reader["Price"],
                                CategoryId = (int)reader["CategoryId"],
                                isDeleted = (bool)reader["isDeleted"],
                                Categories = new Category
                                {
                                    ID = (int)(reader["CategoryID"]),
                                    Name = (string)reader["CategoryName"],
                                    isDeleted = (bool)(reader["CategoryDeleted"])
                                }
                            };
                        }
                    }
                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, product);
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

        public async Task<(int MessageId, string MessageDescription, Product?)> UpdateProductAsync(Product product)
        {
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
                            updatedProduct = new Product
                            {
                                ID = (int)reader["ProductID"],
                                Name = (string)reader["ProductName"],
                                Price = (decimal)reader["Price"],
                                CategoryId = (int)reader["CategoryId"],
                                isDeleted = (bool)reader["isDeleted"],
                                Categories = new Category
                                {
                                    ID = (int)(reader["CategoryID"]),
                                    Name = (string)reader["CategoryName"],
                                    isDeleted = (bool)(reader["CategoryDeleted"])
                                }
                            };
                        }
                    }
                    return ((int)messageIdParam.Value, (string)messageDescriptionParam.Value, updatedProduct);

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
