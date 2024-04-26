namespace ShamsaStoreServer.ViewModels.Order
{
    public class OrderEditViewModel
    {
        public OrderEditViewModel()
        {
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Price { get; set; }

    }
}
