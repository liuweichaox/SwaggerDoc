using System.Collections.Generic;

namespace Sample.Models;

/// <summary>
/// 订单列表返回
/// </summary>
public class GetOrderListResponse
{
    /// <summary>
    /// 订单列表
    /// </summary>
    public List<Order> OrderList { get; set; }
    
    /// <summary>
    /// 总数
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// 总页数
    /// </summary>
    /// <remarks>总页数 = 总数 / 每页数量</remarks>
    public int TotalPage { get; set; }
    
    /// <summary>
    /// 当前页
    /// </summary>
    public int CurrentPage { get; set; }
}
