using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderBooks.Client.Models.OrderBooks;
using OrderBooks.Domain.Services;

namespace OrderBooks.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderBooksController : ControllerBase
    {
        private readonly IOrderBooksService _orderBooksService;
        private readonly IMapper _mapper;

        public OrderBooksController(IOrderBooksService orderBooksService, IMapper mapper)
        {
            _orderBooksService = orderBooksService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllAsync()
        {
            var orderBooks = _orderBooksService.GetAll();

            var model = _mapper.Map<List<OrderBookModel>>(orderBooks);

            return Ok(model);
        }

        [HttpGet("{assetPairId}")]
        public IActionResult GetByAssetPairIdAsync(string assetPairId)
        {
            var orderBook = _orderBooksService.GetByAssetPairId(assetPairId);

            var model = _mapper.Map<OrderBookModel>(orderBook);

            return Ok(model);
        }
    }
}
