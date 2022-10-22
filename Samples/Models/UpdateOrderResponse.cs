using System;

namespace Samples.Models;

/// <summary>
/// 更新订单返回
/// </summary>
public class UpdateOrderResponse
{
    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    public string OrderStatus { get; set; }

    /// <summary>
    /// 订单状态描述
    /// </summary>
    public string OrderStatusDesc { get; set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal OrderAmount { get; set; }
}