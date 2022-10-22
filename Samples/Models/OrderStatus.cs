using System.ComponentModel;

namespace Samples.Models;

/// <summary>
/// 订单状态
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// 未支付
    /// </summary>
    [Description("未支付")] Unpaid = 0,

    /// <summary>
    /// 已支付
    /// </summary>
    [Description("已支付")] Paid = 1,

    /// <summary>
    /// 已发货
    /// </summary>
    [Description("已发货")] Shipped = 2,

    /// <summary>
    /// 已完成
    /// </summary>
    [Description("已完成")] Completed = 3,

    /// <summary>
    /// 已取消
    /// </summary>
    [Description("已取消")] Canceled = 4
}