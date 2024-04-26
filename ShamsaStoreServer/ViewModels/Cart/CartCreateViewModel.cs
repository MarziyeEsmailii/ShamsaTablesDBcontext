using System;

namespace ShamsaStoreServer.ViewModels.Cart
{
    public class CartCreateViewModel
    {
        public CartCreateViewModel()
        {
        }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        public int Count { get; set; }

    }
}
