using System;
using System.Collections.Generic;

namespace CSCAssignment.Models
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> products = new List<Product>();
        private int _nextId = 1;
        public ProductRepository()
        {
            Add(new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 });
            Add(new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M });
            Add(new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M });
        }
        public IEnumerable<Product> GetAll(){
            return products;
        }
        public Product Get(int id){
            return products.Find(p => p.Id == id);
        }

        public Product Add(Product item)
        {
            if(item == null)
                throw new ArgumentNullException("item");
            item.Id = _nextId++;
            products.Add(item);
            return item;
        }

        public void Remove(int id)
        {
            products.RemoveAll(p => p.Id == id);
        }

        public bool Update(Product item)
        {
            if(item == null)
                throw new ArgumentNullException("item");
            int index = products.FindIndex(p => p.Id == item.Id);
            if (index == -1)
                return false;
            products.RemoveAt(index);
            products.Add(item);
            return true;
        }
    }
}