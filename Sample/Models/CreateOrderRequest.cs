using System.ComponentModel.DataAnnotations;

namespace Sample.Models;

/// <summary>
/// 创建订单参数
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// 商品ID
    /// </summary>
    public int GoodsId { get; set; }
    
    /// <summary>
    /// 商品名称
    /// </summary>
    [Required]
    public string GoodsName { get; set; }
    
    /// <summary>
    /// 商品数量
    /// </summary>
    public int GoodsNum { get; set; }
    
    /// <summary>
    /// 订单类型
    /// </summary>
    public OrderType OrderType { get; set; }

    /// <summary>
    /// 收货地址
    /// </summary>
    [Required]
    public Address Address { get; set; }
}