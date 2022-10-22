namespace Samples.Models;

/// <summary>
/// 查询订单详情参数
/// </summary>
public class GetOrderListRequest
{
    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 订单类型
    /// </summary>
    public OrderType OrderType { get; set; }
    
    /// <summary>
    /// 订单状态
    /// </summary>
    public OrderStatus OrderStatus { get; set; }
}