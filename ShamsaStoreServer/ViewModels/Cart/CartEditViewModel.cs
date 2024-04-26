namespace ShamsaStoreServer.ViewModels.Cart
{
    public class CartEditViewModel
    {
        public CartEditViewModel()
        {
            
        }

        public int Id { get; set; }
        public int ProductId { get; set; }

        public int UserId { get; set; }

        public int Count { get; set; }
    }
}
