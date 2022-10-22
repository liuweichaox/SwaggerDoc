using System.ComponentModel.DataAnnotations;

namespace Samples.Models;

/// <summary>
/// 更新订单
/// </summary>
public class UpdateOrderRequest
{
    /// <summary>
    /// 订单号
    /// </summary>
    [Required]
    public string OrderNo { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    public OrderStatus OrderStatus { get; set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal OrderAmount { get; set; }

    /// <summary>
    /// 订单备注
    /// </summary>
    public string OrderRemark { get; set; }
}