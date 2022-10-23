using Microsoft.AspNetCore.Mvc;
using Samples.Models;

namespace Samples.Controllers
{
    /// <summary>
    /// 示例控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="id">订单号</param>
        /// <param name="request">请求参数</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public GetOrderListResponse GetOrderDetails(int id, [FromQuery]GetOrderListRequest request)
        {
            return new GetOrderListResponse();
        }
        
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="request">创建内容</param>
        /// <returns></returns>
        [HttpPost]
        public CreateOrderResponse CreateOrder([FromBody] CreateOrderRequest request)
        {
            return new CreateOrderResponse();
        }
        
        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request">修改内容</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public UpdateOrderResponse UpdateOrder(int id, [FromBody] UpdateOrderRequest request)
        {
            return new UpdateOrderResponse();
        }
        
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id">订单号</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public DeleteOrderResponse DeleteOrder(int id)
        {
            return new DeleteOrderResponse();
        }
    }
}