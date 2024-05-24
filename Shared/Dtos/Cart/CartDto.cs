using System;

namespace ShamsaStoreServer.ViewModels.Cart
{
    public class CartDto
    {
        public CartDto()
        {
        }

        public int Id { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        public int Count { get; set; }

        public string ProductName { get; set; }
    }
}
