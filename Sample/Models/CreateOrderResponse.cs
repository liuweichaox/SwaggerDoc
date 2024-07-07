namespace Sample.Models;

/// <summary>
/// 创建订单返回
/// </summary>
public class CreateOrderResponse
{
    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; }

    /// <summary>
    /// 支付链接
    /// </summary>
    public string PayUrl { get; set; }
    
    /// <summary>
    /// 支付二维码
    /// </summary>
    public string PayQrCode { get; set; }
}
