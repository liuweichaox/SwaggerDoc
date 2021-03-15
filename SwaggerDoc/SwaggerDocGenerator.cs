using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SwaggerDoc.Extensions;
using SwaggerDoc.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerDoc.Helpers
{
    /// <summary>
    /// SwaggerDocGenerator
    /// </summary>
    public class SwaggerDocGenerator : ISwaggerDocGenerator
    {
        /// <summary>
        /// SwaggerGenerator
        /// </summary>
        private readonly SwaggerGenerator _generator;
        /// <summary>
        /// Schemas
        /// </summary>
        private IDictionary<string, OpenApiSchema> Schemas;
        /// <summary>
        /// contentType
        /// </summary>
        const string contentType = "application/json";
        /// <summary>
        /// SwaggerDocGenerator
        /// </summary>
        /// <param name="swagger"></param>
        public SwaggerDocGenerator(SwaggerGenerator swagger)
        {
            _generator = swagger;
        }
        /// <summary>
        /// 生成MarkDown
        /// </summary>
        /// <returns></returns>
        public string GetSwaggerDoc(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("name is null !");
            var document = _generator.GetSwagger(name);
            if (document == null)
                throw new Exception("swagger is null !");
            Schemas = document.Components.Schemas;
            var markDown = new StringBuilder();
            markDown.AppendLine(document?.Info?.Title.H());//文档标题
            markDown.AppendLine(document?.Info?.Description.Ref());//文档描述

            foreach (var path in document.Paths)
            {
                var openApiPath = path.Value;
                var (flag, method, operation) = GetApiOperation(openApiPath);
                if (flag == false)
                    continue;
                var row = new StringBuilder();
                var url = path.Key;
                var title = operation.Summary ?? url;
                var httpMethod = method;
                var query = GetParameters(operation.Parameters);
                var (requestExapmle, requestSchema) = GetRequestBody(operation.RequestBody);
                var (responseExapmle, responseSchema) = GetResponses(operation.Responses);
                row.AppendLine(title.H(2));//接口名称
                row.AppendLine("基本信息".H(3).NewLine());//基本信息
                row.AppendLine($"{"接口地址：".B()}{url}".Li().NewLine());
                row.AppendLine($"{"请求方式：".B()}{httpMethod}".Li().NewLine());
                if (httpMethod == "Post" || httpMethod == "Put")
                {
                    row.AppendLine($"{"请求类型：".B()}{contentType}".Li().NewLine());
                }
                if (string.IsNullOrWhiteSpace(query) == false)//Query
                {
                    row.AppendLine("Query".H(3));
                    row.AppendLine(query);
                }
                if (string.IsNullOrWhiteSpace(requestSchema) == false)//RequestSchema
                {
                    row.AppendLine("RequestSchema".H(3));
                    row.AppendLine(requestSchema.Code());
                }
                if (string.IsNullOrWhiteSpace(requestExapmle) == false)//RequestBody
                {
                    row.AppendLine("RequestBody".H(3));
                    row.AppendLine(requestExapmle.Code());
                }
                if (string.IsNullOrWhiteSpace(responseSchema) == false)//ResponseSchema
                {
                    row.AppendLine("ResponseSchema".H(3));
                    row.AppendLine(responseSchema.Code());
                }
                if (string.IsNullOrWhiteSpace(responseExapmle) == false)//ResponseBody
                {
                    row.AppendLine("ResponseBody".H(3));
                    row.AppendLine(responseExapmle.Code());
                }
                if (string.IsNullOrWhiteSpace(row.ToString()) == false)
                    markDown.AppendLine(row.ToString().Br());
            }
            return markDown.ToString();
        }
        private (bool isSuccesss, string method, OpenApiOperation openApiOperation) GetApiOperation(OpenApiPathItem openApiPathItem)
        {
            var operations = openApiPathItem.Operations;
            OpenApiOperation operation;
            OperationType? operationType = null;
            if (operations.ContainsKey(OperationType.Get))
                operationType = OperationType.Get;
            else if (operations.ContainsKey(OperationType.Post))
                operationType = OperationType.Post;
            else if (operations.ContainsKey(OperationType.Put))
                operationType = OperationType.Put;
            else if (operations.ContainsKey(OperationType.Patch))
                operationType = OperationType.Patch;
            else if (operations.ContainsKey(OperationType.Delete))
                operationType = OperationType.Delete;
            var flag = operations.TryGetValue(operationType.Value, out operation);
            return (flag, operationType.Value.ToString(), operation);
        }
        private string GetParameters(IList<OpenApiParameter> apiParameters)
        {
            string str = null;
            var isFirst = true;
            foreach (var parameter in apiParameters)
            {
                var queryTitle = "|参数名称|参数类型|描述|".NewLine();
                queryTitle += "|:----:|:----:|:----:|".NewLine();
                var queryStr = $"|{parameter.Name}|{parameter.Schema.Type}|{parameter.Description}|".NewLine();
                str += isFirst ? $"{queryTitle}{queryStr}" : queryStr;
                isFirst = false;
            }
            return str;
        }
        private (string exampleJson, string schemaJson) GetRequestBody(OpenApiRequestBody body)
        {
            string exampleJson = null, schemaJson = null;
            if (body != null && body.Content.ContainsKey(contentType))
            {
                var schema = body.Content[contentType].Schema;
                exampleJson += GetExapmple(schema).ToJson();
                schemaJson += GetModelInfo(schema, (id) => GetModelTProc(id)).ToJson();
            }
            return (exampleJson, schemaJson);
        }

        private (string exampleJson, string schemaJson) GetResponses(OpenApiResponses body)
        {
            string exampleJson = null, schemaJson = null;
            if (body != null && body["200"].Content.ContainsKey(contentType))
            {
                var schema = body["200"].Content[contentType].Schema;
                exampleJson += GetExapmple(schema).ToJson();
                schemaJson += GetModelInfo(schema, (id) => GetModelTProc(id, false)).ToJson();
            }
            return (exampleJson, schemaJson);
        }
        private object GetExapmple(OpenApiSchema apiSchema)
        {
            object exapmle = null;
            if (apiSchema.Type == null && apiSchema.Reference != null)//object
            {
                var key = apiSchema?.Reference?.Id;
                exapmle = GetModelExample(key);
            }
            else if (apiSchema.Type == "array" && apiSchema.Items != null)//array
            {
                var key = apiSchema?.Items?.Reference?.Id;
                if (key != null)
                    exapmle = new[] { GetModelExample(key) };
                else if (key == null && apiSchema.Items.Type != null)
                    exapmle = new[] { GetDefaultValue(apiSchema.Items.Type) };
            }
            else
            {
                exapmle = GetDefaultValue(apiSchema.Type);
            }
            return exapmle;
        }
        private object GetModelExample(string key)
        {
            if (key != null && Schemas.ContainsKey(key))
            {
                var schema = Schemas.FirstOrDefault(x => x.Key == key).Value;
                var exapmle = new ModelExample();
                if (schema.Properties.Any())
                {
                    foreach (var item in schema.Properties)
                    {
                        if (item.Value.Reference != null && Schemas.FirstOrDefault(x => x.Key == item.Value.Reference.Id).Value.Enum.Count == 0)
                        {
                            var objKey = item.Value.Reference.Id;
                            exapmle.Add(item.Key, GetModelExample(objKey));
                        }
                        else if (item.Value.Items != null)
                        {
                            var arrayKey = item.Value.Items.Reference.Id;
                            exapmle.Add(item.Key, new[] { GetModelExample(arrayKey) });
                        }
                        else
                        {
                            if (item.Value.Reference != null && Schemas.FirstOrDefault(x => x.Key == item.Value.Reference.Id).Value.Enum.Count != 0)
                                exapmle.Add(item.Key, 0);
                            else
                                exapmle.Add(item.Key, GetDefaultValue(item.Value.Format ?? item.Value.Type));
                        }
                    }
                }
                return exapmle;
            }
            return null;
        }

        private object GetModelInfo(OpenApiSchema apiSchema, Func<string, object> func)
        {
            object info = null;
            var key = "";
            if (apiSchema.Type == null && apiSchema.Reference != null)//object
                key = apiSchema?.Reference?.Id;
            else if (apiSchema.Type == "array" && apiSchema.Items != null)//array
                key = apiSchema?.Items?.Reference?.Id ?? apiSchema.Items.Type;
            else if (apiSchema.Type != null)
                key = apiSchema.Type;
            if (key != null)
                info = func(key);
            return info;
        }
        private object GetModelTProc(string key, bool isShowRequired = true)
        {
            if (key != null)
            {
                if (Schemas.ContainsKey(key))
                {
                    var schema = Schemas.FirstOrDefault(x => x.Key == key).Value;
                    var info = new Dictionary<string, object>();
                    if (schema.Properties.Any())
                    {
                        foreach (var item in schema.Properties)
                        {
                            object obj = item.Value.Format ?? item.Value.Type ?? "object";
                            if (item.Value.Reference != null && Schemas.FirstOrDefault(x => x.Key == item.Value.Reference.Id).Value.Enum.Count == 0)
                            {
                                var objKey = item.Value.Reference.Id;
                                obj = GetModelTProc(objKey, isShowRequired);

                            }
                            else if (item.Value.Items != null)
                            {
                                var arrayKey = item.Value.Items.Reference.Id;
                                obj = GetModelTProc(arrayKey, isShowRequired);
                            }
                            else
                            {
                                if (item.Value.Reference != null && Schemas.FirstOrDefault(x => x.Key == item.Value.Reference.Id).Value.Enum.Count != 0)
                                    obj = item.Value.Reference.Id;
                            }
                            if (isShowRequired)
                            {
                                var requestModelInfo = new RequestModelInfo
                                {
                                    参数类型 = obj,
                                    描述 = item.Value.Description,
                                    是否必传 = schema.Required.Any(x => x == item.Key)
                                };
                                info.Add(item.Key, requestModelInfo);
                            }
                            else
                            {
                                var responseModelInfo = new ResponseModelInfo
                                {
                                    参数类型 = obj,
                                    描述 = item.Value.Description
                                };
                                info.Add(item.Key, responseModelInfo);
                            }

                        }
                    }
                    return info;
                }
                else
                {
                    return key;
                }
            }
            return null;
        }
        private object GetDefaultValue(string type)
        {
            var number = new string[] { "byte", "decimal", "double", "enum", "float", "int32", "int64", "sbyte", "short", "uint", "ulong", "ushort" };
            if (number.Any(x => type == x))
                return 0;
            if (type == "string")
                return "string";
            if (type == "bool" || type == "boolean")
                return false;
            if (type == "date-time")
                return DateTime.Now;
            return null;
        }

        public async Task<MemoryStream> GetSwaggerDocStreamAsync(string name)
        {
            using var stream = new MemoryStream();
            using var sw = new StreamWriter(stream);
            var content = GetSwaggerDoc(name);
            await sw.WriteLineAsync(content);
            return stream;
        }
    }
}
