using System.ComponentModel.DataAnnotations.Schema;

namespace ShamsaStoreServer.Entities
{
    public class Cart
    {
        public Cart()
        {
        }

        public int Id { get; set; }

        public int Count { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public User User { get; set; }
    }
}
