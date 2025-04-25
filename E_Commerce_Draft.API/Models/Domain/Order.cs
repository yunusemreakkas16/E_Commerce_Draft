namespace E_Commerce_Draft.API.Models.Domain
{
    public class Order
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool isDeleted { get; set; }

        // Navigation Properties
        public User Users { get; set; }

        public class OrderDetailParamModel
        {
            public int OrderID { get; set; }
        }
        public class OrderResponseModel
        {
            public int MessageId { get; set; }
            public string MessageDescription { get; set; }
            public Order? Order { get; set; }
        }
    }
}
