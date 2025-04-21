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
        public async Task<Product?> AddProductAsync(Product product)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("usp_CreateProduct", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Name", product.Name));
                    command.Parameters.Add(new SqlParameter("@Price", product.Price));
                    command.Parameters.Add(new SqlParameter("@CategoryId", product.CategoryId));

                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync()) 
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Product
                            {
                                ID = (int)reader["ID"],
                                Name = (string)reader["Name"],
                                Price = (decimal)reader["Price"],
                                CategoryId =(int)reader["CategoryId"],
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something has happened. " + ex.Message);
                return null;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
                await _connection.CloseAsync();

            }

            return null;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            try
            {
                using (SqlCommand command = new SqlCommand("usp_GetAllProducts", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await _connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something has happened. " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
                await _connection.CloseAsync();
            }
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try 
            {
                using (SqlCommand command = new SqlCommand("usp_GetProductById", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ProductId", id));
                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if(await reader.ReadAsync())
                        {
                            return new Product
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
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something has happened. " + ex.Message);
                return null;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
                await _connection.CloseAsync();
            }
            return null;
        }

        public async Task<Product?> UpdateProductAsync(Product product)
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
                    await _connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Product
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

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something has happened. " + ex.Message);
                return null;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
                await _connection.CloseAsync();
            }
            return null;
        }
    }
}
