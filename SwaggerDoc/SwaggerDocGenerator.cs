using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using SwaggerDoc.Extensions;
using SwaggerDoc.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerDoc
{
    /// <summary>
    /// Swagger 文档生成器实现
    /// </summary>
    public class SwaggerDocGenerator : ISwaggerDocGenerator
    {
        /// <summary>
        /// Swagger 文档生成器
        /// </summary>
        private readonly SwaggerGenerator _generator;

        /// <summary>
        /// Schemas
        /// </summary>
        private IDictionary<string, OpenApiSchema> _schemas;

        /// <summary>
        /// ContentType
        /// </summary>
        private const string ContentType = "application/json";

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="swagger"></param>
        public SwaggerDocGenerator(SwaggerGenerator swagger)
        {
            _generator = swagger;
        }

        /// <summary>
        /// 生成 MarkDown
        /// </summary>
        /// <returns></returns>
        public string GetSwaggerDoc(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("name is null !");
            var document = _generator.GetSwagger(name);
            if (document == null)
                throw new Exception("document is null !");
            _schemas = document.Components.Schemas;
            var markDown = new StringBuilder();
            markDown.AppendLine(document?.Info?.Title.H()); //文档标题
            markDown.AppendLine(document?.Info?.Description.Ref()); //文档描述
            foreach (var (url, value) in document.Paths)
            {
                foreach (var operationItem in value.Operations)
                {
                    var operation = operationItem.Value;
                    var method = operationItem.Key.ToString();
                    var row = new StringBuilder();
                    var title = operation.Summary ?? url;

                    var query = GetParameters(operation.Parameters);

                    var (requestExample, requestSchema) = GetRequestBody(operation.RequestBody);

                    var (responseExample, responseSchema) = GetResponses(operation.Responses);

                    row.AppendLine(title.H(2)); //接口名称
                    row.AppendLine("基本信息".H(3).NewLine()); //基本信息
                    row.AppendLine($"{"接口地址：".B()}{url}".Li().NewLine());
                    row.AppendLine($"{"请求方式：".B()}{method}".Li().NewLine());

                    if (method is "Post" or "Put")
                    {
                        row.AppendLine($"{"请求类型：".B()}{ContentType}".Li().NewLine());
                    }

                    if (string.IsNullOrWhiteSpace(query) == false) //Query
                    {
                        row.AppendLine("Parameters".H(3));
                        row.AppendLine(query);
                    }

                    if (string.IsNullOrWhiteSpace(requestSchema) == false) //RequestSchema
                    {
                        row.AppendLine("Request Schema".H(3));
                        row.AppendLine(requestSchema.Code());
                    }

                    if (string.IsNullOrWhiteSpace(requestExample) == false) //RequestBody
                    {
                        row.AppendLine("RequestBody Example".H(3));
                        row.AppendLine(requestExample.Code());
                    }

                    if (string.IsNullOrWhiteSpace(responseSchema) == false) //ResponseSchema
                    {
                        row.AppendLine("Response Schema".H(3));
                        row.AppendLine(responseSchema.Code());
                    }

                    if (string.IsNullOrWhiteSpace(responseExample) == false) //ResponseBody
                    {
                        row.AppendLine("ResponseBody Example".H(3));
                        row.AppendLine(responseExample.Code());
                    }

                    if (string.IsNullOrWhiteSpace(row.ToString()) == false)
                        markDown.AppendLine(row.ToString().Br());
                }
            }

            return markDown.ToString();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="apiParameters"></param>
        /// <returns></returns>
        private static string GetParameters(IEnumerable<OpenApiParameter> apiParameters)
        {
            string str = null;
            var isFirst = true;
            foreach (var parameter in apiParameters)
            {
                var queryTitle = "|参数名称|参数类型|参数位置|描述|".NewLine();
                queryTitle += "|:----:|:----:|:----:|:----:|".NewLine();
                var queryStr =
                    $"|{parameter.Name}|{parameter.Schema.Type ?? parameter.Schema.Reference.Id}|{parameter.In}|{parameter.Description}|"
                        .NewLine();
                str += isFirst ? $"{queryTitle}{queryStr}" : queryStr;
                isFirst = false;
            }

            return str;
        }

        /// <summary>
        /// 获取 RequestBody 参数说明、JSON 示例
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        private (string exampleJson, string schemaJson) GetRequestBody(OpenApiRequestBody body)
        {
            if (body == null || body.Content.ContainsKey(ContentType) == false) return (null, null);
            string exampleJson = null, schemaJson = null;
            var schema = body.Content[ContentType].Schema;
            exampleJson += GetExample(schema).ToJson();
            schemaJson += GetModelInfo(schema, (id) => GetModelInfo(id)).ToJson();
            return (exampleJson, schemaJson);
        }

        /// <summary>
        /// 获取 GetResponses 参数说明、JSON 示例
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        private (string exampleJson, string schemaJson) GetResponses(OpenApiResponses body)
        {
            if (body == null || body["200"].Content.ContainsKey(ContentType) == false) return (null, null);
            string exampleJson = null, schemaJson = null;
            var schema = body["200"].Content[ContentType].Schema;
            exampleJson += GetExample(schema).ToJson();
            schemaJson += GetModelInfo(schema, (id) => GetModelInfo(id, false)).ToJson();
            return (exampleJson, schemaJson);
        }

        /// <summary>
        /// 获取 Body 示例
        /// </summary>
        /// <param name="apiSchema"></param>
        /// <returns></returns>
        private object GetExample(OpenApiSchema apiSchema)
        {
            object example = null;
            if (apiSchema.IsObject(_schemas))
            {
                var key = apiSchema.Reference.Id;
                example = GetExample(key);
            }
            else if (apiSchema.IsArray())
            {
                example = apiSchema.IsBaseTypeArray()
                    ? new[] {GetDefaultValue(apiSchema.Items.Type)}
                    : new[] {GetExample(apiSchema.Items.Reference.Id)};
            }
            else if (apiSchema.IsEnum(_schemas))
            {
                var key = apiSchema.Reference.Id;
                example = GetEnum(key).Select(x => x.Value).Min();
            }
            else
            {
                example = GetDefaultValue(apiSchema.Type);
            }

            return example;
        }

        /// <summary>
        /// 获取枚举的值
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private int[] GetEnumValues(string enumType) => GetEnum(enumType).Select(x => x.Value).ToArray();

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private IEnumerable<OpenApiInteger> GetEnum(string enumType) =>
            GetEnumSchema(enumType).Enum.Select(x => ((OpenApiInteger) x));

        /// <summary>
        /// 获取枚举 Schema
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private OpenApiSchema GetEnumSchema(string enumType) => _schemas.SingleOrDefault(x => x.Key == enumType).Value;

        /// <summary>
        /// 递归获取 Body 示例
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private object GetExample(string key)
        {
            if (key == null || _schemas.ContainsKey(key) == false) return null;
            var schema = _schemas.SingleOrDefault(x => x.Key == key).Value;
            if (schema.Properties.Any() == false) return null;
            var example = new ModelExample();
            foreach (var (s, value) in schema.Properties)
            {
                if (value.IsObject(_schemas))
                {
                    var objKey = value.Reference.Id;
                    example.Add(s, objKey == key ? null : GetExample(objKey));
                }
                else if (value.IsArray())
                {
                    example.Add(s,
                        value.IsBaseTypeArray()
                            ? new[] {GetExample(value.Items.Type)}
                            : new[] {GetExample(value.Items.Reference.Id)});
                }
                else
                {
                    example.Add(s,
                        value.IsEnum(_schemas)
                            ? GetEnumValues(value.Reference.Id).Min()
                            : GetDefaultValue(value.Format ?? value.Type));
                }
            }

            return example;
        }

        /// <summary>
        /// 获取 Body 参数说明
        /// </summary>
        /// <param name="apiSchema"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private object GetModelInfo(OpenApiSchema apiSchema, Func<string, object> func)
        {
            object info = null;
            var key = "";
            if (apiSchema.IsObject(_schemas) || apiSchema.IsEnum(_schemas))
                key = apiSchema.Reference.Id;
            else if (apiSchema.IsArray())
                key = apiSchema.Items.Type ?? apiSchema.Items.Reference.Id;
            else if (apiSchema.IsBaseType())
                key = apiSchema.Type;
            if (key != null)
                info = func(key);
            return info;
        }

        /// <summary>
        /// 递归获取 Body 参数说明
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isShowRequired"></param>
        /// <returns></returns>
        private object GetModelInfo(string key, bool isShowRequired = true)
        {
            if (key == null) return null;
            if (_schemas.ContainsKey(key) == false) return key;
            var schema = _schemas.SingleOrDefault(x => x.Key == key).Value;
            if (schema.Properties.Any() == false)
                return new EnumInfo()
                {
                    枚举范围 = GetEnumValues(key),
                    枚举描述 = schema.Description,
                    枚举类型 = schema.Format,
                    枚举名称 = key
                };
            var properties = new Dictionary<string, object>();
            foreach (var item in schema.Properties)
            {
                object obj = "object";
                if (item.Value.IsObject(_schemas))
                {
                    var objKey = item.Value.Reference.Id;
                    obj = objKey == key ? objKey : GetModelInfo(objKey, isShowRequired);
                }
                else if (item.Value.IsArray())
                {
                    var arrayKey = "";
                    arrayKey = item.Value.IsBaseTypeArray() ? item.Value.Items.Type : item.Value.Items.Reference.Id;
                    obj = new[] {GetModelInfo(arrayKey, isShowRequired)};
                }
                else if (item.Value.IsEnum(_schemas))
                {
                    var enumKey = item.Value.Reference.Id;
                    var enumObj = GetEnumSchema(enumKey);
                    obj = new EnumInfo()
                    {
                        枚举范围 = GetEnumValues(enumKey),
                        枚举类型 = enumObj.Format,
                        枚举名称 = enumKey,
                        枚举描述 = enumObj.Description
                    };
                }
                else
                {
                    obj = item.Value.Format ?? item.Value.Type;
                }

                if (isShowRequired)
                {
                    var requestModelInfo = new RequestModelInfo
                    {
                        参数类型 = obj,
                        描述 = item.Value.Description,
                        是否必传 = schema.Required.Any(x => x == item.Key),
                        可空类型 = item.Value.Nullable
                    };
                    properties.Add(item.Key, requestModelInfo);
                }
                else
                {
                    var responseModelInfo = new ResponseModelInfo
                    {
                        参数类型 = obj,
                        描述 = item.Value.Description,
                        可空类型 = item.Value.Nullable
                    };
                    properties.Add(item.Key, responseModelInfo);
                }
            }

            return properties;
        }

        /// <summary>
        /// 获取类型默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object GetDefaultValue(string type)
        {
            var number = new string[]
            {
                "byte", "decimal", "double", "enum", "float", "int32", "int64", "sbyte", "short", "uint", "ulong",
                "ushort"
            };
            if (number.Any(x => type == x)) return 0;
            switch (type)
            {
                case "string":
                    return "string";
                case "bool":
                case "boolean":
                    return false;
                case "date-time":
                    return DateTime.Now;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取 MarkDown 文件流
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<MemoryStream> GetSwaggerDocStreamAsync(string name)
        {
            await using var stream = new MemoryStream();
            await using var sw = new StreamWriter(stream);
            var content = GetSwaggerDoc(name);
            await sw.WriteLineAsync(content);
            return stream;
        }
    }
}