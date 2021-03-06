using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using OrderBooks.Domain.Entities;
using OrderBooks.Domain.Services;

namespace OrderBooks.Services
{
    public class OrderBooksService : IOrderBooksService
    {
        private readonly ConcurrentDictionary<string, OrderBook> _orderBooks =
            new ConcurrentDictionary<string, OrderBook>();
        
        public IReadOnlyList<OrderBook> GetAll()
        {
            return _orderBooks.Values.ToList();
        }

        public OrderBook GetByAssetPairId(string assetPairId)
        {
            _orderBooks.TryGetValue(assetPairId, out var orderBook);

            return orderBook;
        }
        
        public void Update(OrderBook orderBook)
        {
            _orderBooks.AddOrUpdate(orderBook.AssetPairId, orderBook, (key, value) => orderBook);
        }
    }
}
