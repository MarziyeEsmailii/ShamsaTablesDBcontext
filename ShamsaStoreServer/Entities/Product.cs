using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShamsaStoreServer.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public string ImageFileName { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public virtual Category Category { get; set; }

        public List<Order> Orders { get; set; }

        public List<Cart> Carts { get; set; }
    }
}
