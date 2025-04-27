namespace E_Commerce_Draft.API.Models.Domain
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; } 
        public bool isDeleted { get; set; }

    }

    public class CategoryDetailParamModel
    {
        public int ID { get; set; }
    }

    public class CategoryResponseModel
    {
        public int MessageId { get; set; }
        public string MessageDescription { get; set; }
        public Category? Category { get; set; }
    }

    public class CategoryListResponseModel
    {
        public int MessageId { get; set; }
        public string MessageDescription { get; set; } 
        public List<Category>? Categories { get; set; } 
    }
}
