namespace E_Commerce_Draft.API.Models.Domain
{
    public class User
    {
        public int ID { get; set; } 
        public string Name { get; set; }  
        public string Email { get; set; }  
        public string PasswordHash { get; set; }  
        public bool isDeleted { get; set; }  

        public class UserDetailParamModel
        {
            public int ID { get; set; }
        }
        public class UserResponseModel
        {
            public int MessageId { get; set; }
            public string MessageDescription { get; set; } 
            public User? User { get; set; }
        }
        public class UserListResponseModel
        {
            public int MessageId { get; set; }
            public string MessageDescription { get; set; }
            public List<User>? Users { get; set; }
        }

    }
}
