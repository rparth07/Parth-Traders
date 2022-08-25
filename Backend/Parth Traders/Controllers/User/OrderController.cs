using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Parth_Traders.Domain.Entity.User;
using Parth_Traders.Domain.Enums;
using Parth_Traders.Dto.User;
using Parth_Traders.Service.Services.User.UserInterface;

namespace Parth_Traders.Controllers.User
{
    [ApiController]
    [Route("API/orders")]
    public class OrderController : ControllerBase
    {
        public readonly IOrderService _orderService;
        public readonly IOrderHelperService _orderHelperService;
        public readonly IMapper _mapper;

        public OrderController(IOrderService orderService,
                               IOrderHelperService orderHelperService,
                               IMapper mapper)
        {
            _orderService = orderService ??
                throw new ArgumentNullException(nameof(orderService));
            _orderHelperService = orderHelperService ??
                throw new ArgumentNullException(nameof(orderHelperService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult AddOrder(OrderDto orderForCreation)
        {
            var order = _mapper.Map<Order>(orderForCreation);

            var orderToAdd = _orderHelperService.MapOrderPropertiesToOrder(order);

            Order addedOrder = _orderService.AddOrder(orderToAdd);

            OrderDto orderToReturn = _mapper.Map<OrderDto>(addedOrder);

            return Ok(orderToReturn);
        }

        [HttpGet("{customerName}/{orderId}", Name = "GetOrder")]
        public IActionResult GetOrder(string customerName, long orderId)
        {
            OrderDto orderFromRepo = _mapper.Map<OrderDto>(_orderService.GetOrderById(orderId));
            return Ok(orderFromRepo);
        }

        [HttpGet("{customerName}/latest")]
        public IActionResult GetLatestOrderForCustomer(string customerName)
        {
            var order = _mapper.Map<OrderDto>(_orderService
                .GetLatestOrderForCustomer(customerName));

            return Ok(order);
        }

        [HttpGet("{customerName}")]
        public IActionResult GetAllOrdersForCustomer(string customerName)
        {
            var orders = _mapper.Map<List<OrderDto>>(_orderService
                .GetAllOrdersForCustomer(customerName));

            return Ok(orders);
        }

        [HttpGet("{customerName}/status/{orderStatus}")]
        public IActionResult GetAllOrdersForCustomerWithStatus(string customerName,
                                                               OrderStatus orderStatus)
        {
            var orders = _mapper.Map<List<OrderDto>>(_orderService
                .GetAllOrdersForCustomerWithStatus(customerName, orderStatus));

            return Ok(orders);
        }

        [HttpDelete("{customerName}/{orderId}")]
        public IActionResult DeleteOrder(long orderId)
        {
            _orderService.CancelOrder(orderId);
            return NoContent();
        }
    }
}
