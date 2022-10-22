using System.ComponentModel.DataAnnotations;

namespace Samples.Models;

/// <summary>
/// 地址
/// </summary>
public class Address
{
    /// <summary>
    /// 国家
    /// </summary>
    public string Country { get; set; }
    
    /// <summary>
    /// 城市
    /// </summary>
    public string City { get; set; }
    
    /// <summary>
    /// 街道
    /// </summary>
    public string Street { get; set; }
    
    /// <summary>
    /// 邮编
    /// </summary>
    public string ZipCode { get; set; }
}