using System;
using System.Linq;

namespace SwaggerDoc.Extensions
{
    /// <summary>
    /// MarkDown文档扩展
    /// </summary>
    public static class MarkDownExtensions
    {
        /// <summary>
        /// 标题(1~6级)
        /// </summary>
        /// <param name="s">字符串内容</param>
        /// <param name="len">标题级别</param>
        /// <returns></returns>
        public static string H(this string s, int len = 1) => $"{(len is <= 6 and > 0 ? string.Join("", Enumerable.Range(0, len).Select(x => "#")) : "#")} {s}";
        
        /// <summary>
        /// 加粗
        /// </summary>
        /// <param name="s">字符串内容</param>
        /// <returns></returns>
        public static string B(this string s) => $"**{s}**";
        
        /// <summary>
        /// 斜体
        /// </summary>
        /// <param name="s">字符串内容</param>
        /// <returns></returns>
        public static string I(this string s) => $"*{s}*";
        
        /// <summary>
        /// 无需列表
        /// </summary>
        /// <param name="s">字符串内容</param>
        /// <param name="len">缩进</param>
        /// <returns></returns>
        public static string Li(this string s, int len = 0) => $"{string.Join("", Enumerable.Range(0, len).Select(x => " "))}- {s}";
        
        /// <summary>
        /// 引用
        /// </summary>
        /// <param name="s">字符串内容</param>
        /// <param name="len">引用等级</param>
        /// <returns></returns>
        public static string Ref(this string s,int len=1) => $"{string.Join("", Enumerable.Range(0, len).Select(x => ">"))} {s}";
        
        /// <summary>
        /// 下划线
        /// </summary>
        /// <param name="s">字符串内容</param>
        /// <returns></returns>
        public static string Line(this string s) => $"***";
        
        /// <summary>
        /// 换行
        /// </summary>
        /// <param name="s">字符串内容</param>
        /// <returns></returns>
        public static string NewLine(this string s) => $"{s}{Environment.NewLine}";
        
        /// <summary>
        /// 超链接
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="url">超链接地址</param>
        /// <param name="title">链接title</param>
        /// <returns></returns>
        public static string Link(this string name, string url, string title = "") => $"[{name}]({url} {title})";
        
        /// <summary>
        /// 换行
        /// </summary>
        /// <param name="s">字符串内容</param>
        /// <returns></returns>
        public static string Br(this string s) => $"{s}<br/>";
        
        /// <summary>
        /// 代码块
        /// </summary>
        /// <param name="s">字符串内容</param>
        /// <param name="type">代码语言类型</param>
        /// <returns></returns>
        public static string Code(this string s, string type = "json") => $"```json{Environment.NewLine}{s}{Environment.NewLine}```{Environment.NewLine}";
    }
}
