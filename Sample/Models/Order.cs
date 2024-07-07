using System;

namespace Sample.Models;

/// <summary>
/// 订单
/// </summary>
public class Order
{
    /// <summary>
    /// 订单编号
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; }
    
    /// <summary>
    /// 商品ID
    /// </summary>
    public int GoodsId { get; set; }

    /// <summary>
    /// 商品名称
    /// </summary>
    public string GoodsName { get; set; }

    /// <summary>
    /// 商品数量
    /// </summary>
    public int GoodsNum { get; set; }
    
    /// <summary>
    /// 商品单价
    /// </summary>
    public int GoodsPrice { get; set; }

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
    /// 订单支付时间
    /// </summary>
    public DateTime PayTime { get; set; }
    
    /// <summary>
    /// 收货地址
    /// </summary>
    public Address Address { get; set; }
    
    /// <summary>
    /// 订单创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 订单更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
    
    /// <summary>
    /// 订单备注
    /// </summary>
    public string Remark { get; set; }
}
