namespace ShamsaStoreServer.ViewModels.Order
{
    public class OrderCreateViewModel
    {
        public OrderCreateViewModel()
        {
        }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Price { get; set; }

    }
}
