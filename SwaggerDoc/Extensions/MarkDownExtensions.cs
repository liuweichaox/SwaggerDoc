using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDoc.Extensions
{
    /// <summary>
    /// MarkDownExtensions
    /// </summary>
    public static class MarkDownExtensions
    {
        public static string H1(this string s) => $"# {s}";
        public static string H2(this string s) => $"## {s}";
        public static string H3(this string s) => $"### {s}";
        public static string H4(this string s) => $"#### {s}";
        public static string H5(this string s) => $"##### {s}";
        public static string H6(this string s) => $"###### {s}";
        public static string B(this string s) => $"**{s}**";
        public static string I(this string s) => $"*{s}*";
        public static string Li(this string s, int len = 0) => $"{string.Join("", Enumerable.Range(0, len).Select(x => " "))}- {s}";
        public static string Ref1(this string s) => $"> {s}";
        public static string Ref2(this string s) => $">> {s}";
        public static string Ref3(this string s) => $">>> {s}";
        public static string Line(this string s) => $"***";
        public static string NewLine(this string s) => $"{s}{Environment.NewLine}";
        public static string Link(this string name, string url, string title = "") => $"[{name}]({url},{title})";
        public static string Br(this string s) => $"{s}<br/>";
        public static string Code(this string s, string type = "json") => $"```json{Environment.NewLine}{s}{Environment.NewLine}```{Environment.NewLine}";
    }
}
