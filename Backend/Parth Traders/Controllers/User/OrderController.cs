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

        [HttpGet("{userName}/{orderId}", Name = "GetOrder")]
        public IActionResult GetOrder(string userName, long orderId)
        {
            OrderDto orderFromRepo = _mapper.Map<OrderDto>(_orderService.GetOrderById(orderId));
            return Ok(orderFromRepo);
        }

        [HttpGet("{userName}/latest")]
        public IActionResult GetLatestOrderForCustomer(string userName)
        {
            var order = _mapper.Map<OrderDto>(_orderService
                .GetLatestOrderForCustomer(userName));

            return Ok(order);
        }

        [HttpGet("{userName}")]
        public IActionResult GetAllOrdersForCustomer(string userName)
        {
            var orders = _mapper.Map<List<OrderDto>>(_orderService
                .GetAllOrdersForCustomer(userName));

            return Ok(orders);
        }

        [HttpGet("{userName}/status/{orderStatus}")]
        public IActionResult GetAllOrdersForCustomerWithStatus(string userName,
                                                               OrderStatus orderStatus)
        {
            var orders = _mapper.Map<List<OrderDto>>(_orderService
                .GetAllOrdersForCustomerWithStatus(userName, orderStatus));

            return Ok(orders);
        }

        [HttpDelete("{userName}/{orderId}")]
        public IActionResult DeleteOrder(long orderId)
        {
            _orderService.CancelOrder(orderId);
            return NoContent();
        }
    }
}
