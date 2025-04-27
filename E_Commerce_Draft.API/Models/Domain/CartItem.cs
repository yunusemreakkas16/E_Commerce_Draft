namespace E_Commerce_Draft.API.Models.Domain
{
    public class CartItem
    {
        public int ID { get; set; } 
        public int UserId { get; set; } 
        public int ProductId { get; set; } 
        public int Quantity { get; set; }  

        // Navigation Properties
        public User User { get; set; } 
        public Product Product { get; set; }

        public class CartItemDetailParamModel
        {
            public int ID { get; set; }
        }

        public class CartItemResponseModel
        {
            public CartItem? CartItem { get; set; } = null;
            public int MessageId { get; set; } = 0;
            public string? MessageDescription { get; set; } = null;
        }
        public class GetAllCartItemsResponseModel
        {
            public int MessageId { get; set; }          
            public string MessageDescription { get; set; } 
            public List<CartItem> CartItems { get; set; } 
        }

        public class GetCartItemsByUserIdResponseModel
        {
            public int MessageId { get; set; }
            public string MessageDescription { get; set; }
            public List<CartItem> CartItems { get; set; }
        }


    }
}