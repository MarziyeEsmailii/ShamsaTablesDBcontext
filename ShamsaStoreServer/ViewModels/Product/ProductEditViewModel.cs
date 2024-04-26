﻿namespace ShamsaStoreServer.ViewModels.Product
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }

        public int CategoryId { get; set; }

        public string ImageFileName { get; set; }
    }
}
