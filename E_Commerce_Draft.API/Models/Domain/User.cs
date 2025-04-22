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
    }
}
