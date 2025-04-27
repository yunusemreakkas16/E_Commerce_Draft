namespace E_Commerce_Draft.API.Models.Domain
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool isDeleted { get; set; }

        // Navigation Property
        public Category Categories { get; set; }

        public class ProductDetailParamModel
        {
            public int ID { get; set; }
        }

        public class ProductResponseModel
        {
            public int MessageId { get; set; }
            public string MessageDescription { get; set; } 
            public Product? Product { get; set; }
        }

        public class GetAllProductsResponseModel
        {
            public int MessageId { get; set; } 
            public string MessageDescription { get; set; } 
            public List<Product>? Products { get; set; } 
        }

    }
}
