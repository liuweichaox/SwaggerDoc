using System.ComponentModel.DataAnnotations;

namespace Samples.Models;

/// <summary>
/// 更新订单
/// </summary>
public class UpdateOrderRequest
{
    /// <summary>
    /// 订单编号
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 订单号
    /// </summary>
    [Required]
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 商品数量
    /// </summary>
    public int GoodsNum { get; set; }
    
    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    public OrderStatus OrderStatus { get; set; }
    
    /// <summary>
    /// 订单类型
    /// </summary>
    public OrderType OrderType { get; set; }
    
    /// <summary>
    /// 订单备注
    /// </summary>
    public string Remark { get; set; }
    
    /// <summary>
    /// 收货地址
    /// </summary>
    public Address Address { get; set; }
}