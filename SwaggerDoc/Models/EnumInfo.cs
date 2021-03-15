using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerDoc.Models
{
    /// <summary>
    /// 枚举信息
    /// </summary>
    public class EnumInfo
    {
        /// <summary>
        /// 枚举类型
        /// </summary>
        public string 枚举名称 { get; set; }
        /// <summary>
        /// 枚举值类型
        /// </summary>
        public string 枚举类型 { get; set; }
        /// <summary>
        /// 枚举值
        /// </summary>
        public int[] 枚举范围 { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string 枚举描述 { get; set; }
    }
}
