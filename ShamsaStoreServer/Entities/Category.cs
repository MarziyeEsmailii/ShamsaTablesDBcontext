using System.Collections;
using System.Collections.Generic;

namespace ShamsaStoreServer.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageFileName { get; set; }

        public List<Product> Products { get; set; }
    }
}
