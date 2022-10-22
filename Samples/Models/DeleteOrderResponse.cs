namespace Samples.Models;

/// <summary>
/// 删除订单返回
/// </summary>
public class DeleteOrderResponse
{
    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 订单状态
    /// </summary>
    public OrderStatus OrderStatus { get; set; }
}