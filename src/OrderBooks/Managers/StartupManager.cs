using System;
using System.Linq;
using System.Threading.Tasks;
using MatchingEngine.Client;
using OrderBooks.Domain.Entities;
using OrderBooks.Domain.Handlers;
using OrderBooks.Rabbit.Subscribers;

namespace OrderBooks.Managers
{
    public class StartupManager
    {
        private readonly OrderBooksSubscriber _orderBooksSubscriber;
        private readonly IMatchingEngineClient _matchingEngineClient;
        private readonly IOrderBooksHandler _orderBooksHandler;

        public StartupManager(
            OrderBooksSubscriber orderBooksSubscriber,
            IMatchingEngineClient matchingEngineClient,
            IOrderBooksHandler orderBooksHandler)
        {
            _orderBooksSubscriber = orderBooksSubscriber;
            _matchingEngineClient = matchingEngineClient;
            _orderBooksHandler = orderBooksHandler;
        }

        public async Task StartAsync()
        {
            var orderBooks = await _matchingEngineClient.OrderBooks.GetAllAsync();

            foreach (var orderBook in orderBooks)
            {
                var type = orderBook.IsBuy
                    ? LimitOrderType.Buy
                    : LimitOrderType.Sell;

                var limitOrders = orderBook.Levels
                    .Select(level => new LimitOrder
                    {
                        Id = level.OrderId,
                        Price = level.Price,
                        Volume = Math.Abs(level.Volume),
                        WalletId = level.WalletId,
                        Type = type
                    })
                    .ToList();

                if (orderBook.IsBuy)
                    _orderBooksHandler.HandleBuy(orderBook.AssetPairId, orderBook.Timestamp, limitOrders);
                else
                    _orderBooksHandler.HandleSell(orderBook.AssetPairId, orderBook.Timestamp, limitOrders);
            }

            _orderBooksSubscriber.Start();
        }
    }
}
