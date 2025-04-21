namespace E_Commerce_Draft.API.Models.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool isDeleted { get; set; }

        // Navigation Properties
        public User Users { get; set; }
    }
}
