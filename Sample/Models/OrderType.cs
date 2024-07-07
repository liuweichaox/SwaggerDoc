using System.ComponentModel;

namespace Sample.Models;

/// <summary>
/// 订单类型
/// </summary>
public enum OrderType
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")] Unknown = 0,

    /// <summary>
    /// 一般订单
    /// </summary>
    [Description("一般订单")] Normal = 1,

    /// <summary>
    /// 促销订单
    /// </summary>
    [Description("促销订单")] Promotion = 2,

    /// <summary>
    /// 退货订单
    /// </summary>
    [Description("退货订单")] Return = 3,

    /// <summary>
    /// 换货订单
    /// </summary>
    [Description("换货订单")] Exchange = 4,

    /// <summary>
    /// 退款订单
    /// </summary>
    [Description("退款订单")] Refund = 5
}