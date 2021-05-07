using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;

namespace Pizza.Data
{
    public class ToppingEntity : TableEntity
    {
        public ToppingEntity()
        {
            PartitionKey = "toppings";
        }
        
        public ToppingEntity(string id, string name, double price, int stockCount) : this()
        {
            Id = id;
            Name = name;
            Price = price;
            StockCount = stockCount;
        }

        [IgnoreProperty]
        public string Id
        {
            get => RowKey;
            set => RowKey = value;
        }
        public string Name { get; set; }
        public double Price { get; set; }
        public int StockCount { get; set; }
    }
}
