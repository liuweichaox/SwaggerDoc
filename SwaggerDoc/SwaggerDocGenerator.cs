﻿using Microsoft.OpenApi.Any;
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
            {
                throw new Exception("name is null !");
            }

            var document = _generator.GetSwagger(name);
            if (document == null)
            {
                throw new Exception("document is null !");
            }

            _schemas = document.Components.Schemas;
            var markDown = new StringBuilder();
            markDown.AppendLine(document.Info?.Title.H()); //文档标题
            markDown.AppendLine(document.Info?.Description.Ref()); //文档描述
            foreach (var (url, value) in document.Paths)
            {
                foreach (var (key, operation) in value.Operations)
                {
                    var method = key.ToString();
                    var row = new StringBuilder();
                    var title = operation.Summary ?? url;
                    var parameters = GetParameters(operation.Parameters);
                    var (requestExample, requestSchema) = GetRequest(operation.RequestBody);
                    var (responseExample, responseSchema) = GetResponse(operation.Responses);
                    row.AppendLine(title.H(2)); //接口名称
                    row.AppendLine("基本信息".H(3).NewLine()); //基本信息
                    row.AppendLine($"{"接口地址：".B()}{url}".Li().NewLine());
                    row.AppendLine($"{"请求方式：".B()}{method}".Li().NewLine());

                    if (method is "Post" or "Put")
                    {
                        row.AppendLine($"{"请求类型：".B()}{ContentType}".Li().NewLine());
                    }

                    if (string.IsNullOrWhiteSpace(parameters) == false) //Parameters
                    {
                        row.AppendLine("请求参数(Parameters)：".H(3));
                        row.AppendLine(parameters);
                    }

                    if (string.IsNullOrWhiteSpace(requestSchema) == false) //RequestSchema
                    {
                        row.AppendLine("请求参数(Body)：".H(3));
                        row.AppendLine(requestSchema.Code());
                    }

                    if (string.IsNullOrWhiteSpace(requestExample) == false) //RequestExample
                    {
                        row.AppendLine("请求示例：".H(3));
                        row.AppendLine(requestExample.Code());
                    }

                    if (string.IsNullOrWhiteSpace(responseSchema) == false) //ResponseSchema
                    {
                        row.AppendLine("返回参数：".H(3));
                        row.AppendLine(responseSchema.Code());
                    }

                    if (string.IsNullOrWhiteSpace(responseExample) == false) //ResponseBody
                    {
                        row.AppendLine("返回示例：".H(3));
                        row.AppendLine(responseExample.Code());
                    }

                    if (string.IsNullOrWhiteSpace(row.ToString()) == false)
                    {
                        markDown.AppendLine(row.ToString().Br());
                    }
                }
            }

            return markDown.ToString();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="apiParameters"></param>
        /// <returns></returns>
        private string GetParameters(IEnumerable<OpenApiParameter> apiParameters)
        {
            string str = null;
            var isFirst = true;
            var queryTitle = "|参数名称|参数类型|参数位置|描述|".NewLine();
            queryTitle += "|:----:|:----:|:----:|:----:|".NewLine();
            foreach (var parameter in apiParameters)
            {
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
        private (string exampleJson, string schemaJson) GetRequest(OpenApiRequestBody body)
        {
            if (body == null || body.Content.ContainsKey(ContentType) == false)
            {
                return (null, null);
            }

            string exampleJson = null, schemaJson = null;
            var schema = body.Content[ContentType].Schema;
            exampleJson += GetExample(schema).ToJson();
            schemaJson += AnalyzeFiled(schema, (objKey) => GetFiledDetails(objKey, ModelType.Request)).ToJson();
            return (exampleJson, schemaJson);
        }

        /// <summary>
        /// 获取 GetResponses 参数说明、JSON 示例
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        private (string exampleJson, string schemaJson) GetResponse(OpenApiResponses body)
        {
            if (body == null || body["200"].Content.ContainsKey(ContentType) == false)
            {
                return (null, null);
            }

            string exampleJson = null, schemaJson = null;
            var schema = body["200"].Content[ContentType].Schema;
            exampleJson += GetExample(schema).ToJson();
            schemaJson += AnalyzeFiled(schema, (objKey) => GetFiledDetails(objKey, ModelType.Response)).ToJson();
            return (exampleJson, schemaJson);
        }

        /// <summary>
        /// 获取 Body 示例
        /// </summary>
        /// <param name="apiSchema"></param>
        /// <returns></returns>
        private object GetExample(OpenApiSchema apiSchema)
        {
            object example;
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
        /// 分析参数,获取参数说明
        /// </summary>
        /// <param name="apiSchema"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private object AnalyzeFiled(OpenApiSchema apiSchema, Func<string, object> func)
        {
            object info = null;
            var key = "";
            if (apiSchema.IsObject(_schemas) || apiSchema.IsEnum(_schemas))
            {
                key = apiSchema.Reference.Id;
            }
            else if (apiSchema.IsArray())
            {
                key = apiSchema.Items.Type ?? apiSchema.Items.Reference.Id;
            }
            else if (apiSchema.IsBaseType())
            {
                key = apiSchema.Type;
            }

            if (key != null)
            {
                info = func(key);
            }

            return info;
        }

        /// <summary>
        /// 递归获取 Body 参数说明
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        private object GetFiledDetails(string key, ModelType modelType)
        {
            if (key == null) return null;
            if (_schemas.ContainsKey(key) == false)
            {
                return key;
            }

            var schema = _schemas.SingleOrDefault(x => x.Key == key).Value;
            if (schema.Properties.Any() == false)
            {
                return new EnumInfo()
                {
                    枚举范围 = GetEnumValues(key),
                    枚举描述 = schema.Description,
                    枚举类型 = schema.Format,
                    枚举名称 = key
                };
            }

            var properties = new Dictionary<string, object>();
            foreach (var (s, value) in schema.Properties)
            {
                object obj;
                if (value.IsObject(_schemas))
                {
                    var objKey = value.Reference.Id;
                    obj = objKey == key ? objKey : GetFiledDetails(objKey, modelType);
                }
                else if (value.IsArray())
                {
                    var arrayKey = value.IsBaseTypeArray() ? value.Items.Type : value.Items.Reference.Id;
                    obj = new[] {GetFiledDetails(arrayKey, modelType)};
                }
                else if (value.IsEnum(_schemas))
                {
                    var enumKey = value.Reference.Id;
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
                    obj = value.Format ?? value.Type;
                }

                if (modelType == ModelType.Request)
                {
                    var requestModelInfo = new RequestModelInfo
                    {
                        参数类型 = obj,
                        描述 = value.Description,
                        是否必传 = schema.Required.Any(x => x == s),
                        可空类型 = value.Nullable
                    };
                    properties.Add(s, requestModelInfo);
                }
                else
                {
                    var responseModelInfo = new ResponseModelInfo
                    {
                        参数类型 = obj,
                        描述 = value.Description,
                        可空类型 = value.Nullable
                    };
                    properties.Add(s, responseModelInfo);
                }
            }

            return properties;
        }

        /// <summary>
        /// 获取类型默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object GetDefaultValue(string type)
        {
            var number = new[]
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
            await using var sw = new StreamWriter(stream, Encoding.UTF8);
            var content = GetSwaggerDoc(name);
            await sw.WriteLineAsync(content);
            return stream;
        }
    }
}