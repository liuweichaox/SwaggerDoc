using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerDoc;

/// <summary>
///  Swagger 枚举过滤器
/// </summary>
public class SwaggerEnumFilter : IDocumentFilter
{
    /// <summary>
    /// 枚举所在程序集
    /// </summary>
    private readonly Assembly[] _assemblies;

    public SwaggerEnumFilter(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    public void Apply(Microsoft.OpenApi.Models.OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var dict = GetAllEnum();

        foreach (var (typeName, property) in swaggerDoc.Components.Schemas)
        {
            if (property.Enum is not {Count: > 0}) continue;
            var itemType = dict.ContainsKey(typeName) ? dict[typeName] : null;
            var list = property.Enum.Cast<OpenApiInteger>().ToList();
            property.Description += DescribeEnum(itemType, list);
        }
    }

    private Dictionary<string, Type> GetAllEnum()
    {
        var types = _assemblies.SelectMany(x => x.GetTypes());

        return types.Where(item => item.IsEnum).ToDictionary(item => item.Name);
    }

    private string DescribeEnum(Type type, List<OpenApiInteger> enums)
    {
        var enumDescriptions = new List<string>();
        foreach (var item in enums)
        {
            if (type == null) continue;
            var value = Enum.Parse(type, item.Value.ToString());
            var desc = GetDescription(type, value);

            enumDescriptions.Add(string.IsNullOrEmpty(desc)
                ? $"{item.Value.ToString()}:{Enum.GetName(type, value)}; "
                : $"{item.Value.ToString()}:{Enum.GetName(type, value)},{desc}; ");
        }

        return "  " + string.Join("  ", enumDescriptions);
    }

    private string GetDescription(Type t, object value)
    {
        foreach (var mInfo in t.GetMembers())
        {
            if (mInfo.Name != t.GetEnumName(value)) continue;
            foreach (var attr in Attribute.GetCustomAttributes(mInfo))
            {
                if (attr.GetType() == typeof(DescriptionAttribute))
                {
                    return ((DescriptionAttribute) attr).Description;
                }
            }
        }

        return string.Empty;
    }
}