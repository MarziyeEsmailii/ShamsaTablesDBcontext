using System.Collections.Generic;

namespace ShamsaStoreServer.Entities
{
    public class User
    {
        public User()
        {
        }

        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<Order> Orders { get; set; }

        public List<Cart> Carts { get; set; }
    }
}
