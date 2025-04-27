namespace E_Commerce_Draft.API.Models.Domain
{
    public class OrderDetail
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int OrderID { get; set; }
        public int Quantity { get; set; }

        // Navigation Properties
        public Product Product { get; set; } 
        public Order Order { get; set; }

        public class OrderDetailDetailParamModel
        {
            public int OrderID { get; set; }
            public int ProductID { get; set; }
        }

        public class UpdateOrderDetailRequestModel
        {
            public int OrderId { get; set; }
            public int ProductId { get; set; }
            public int NewQuantity { get; set; }
        }

        public class OrderDetailResponseModel
        {
            public int MessageId { get; set; }
            public string MessageDescription { get; set; }
            public OrderDetail? OrderDetail { get; set; }
        }
        public class GetAllOrderDetailsResponseModel
        {
            public int MessageId { get; set; }
            public string MessageDescription { get; set; }
            public List<OrderDetail> OrderDetails { get; set; }
        }

    }
}