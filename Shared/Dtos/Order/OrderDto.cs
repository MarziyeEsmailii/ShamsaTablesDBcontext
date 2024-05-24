namespace ShamsaStoreServer.ViewModels.Order
{
    public class OrderDto
    {
        public OrderDto()
        {
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Price { get; set; }

        public string ProductName { get; set; }
        public int Count { get; set; }
    }
}
