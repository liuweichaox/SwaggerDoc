# SwaggerDoc 简介

SwaggerDoc 是基于 Swashbuckle.AspNetCore 类库的离线文档生成工具。该工具以 JSON 结构描述 API 参数说明，并支持枚举类型的描述。用户可以导出 Markdown 格式的文档，方便根据需求转换为其他文件格式。

项目地址：[SwaggerDoc GitHub](https://github.com/liuweichaox/SwaggerDoc)

## 1. SwaggerDoc 引用

### .NET CLI

要引用 SwaggerDoc，请使用以下命令：

```bash
dotnet add package SwaggerDoc --version 1.0.1
```

## 2. Startup 配置

### 注册 SwaggerDoc 服务

在 `Startup.cs` 文件中，注册 SwaggerDoc 服务，以支持 Markdown 生成：

```csharp
services.AddSwaggerDoc(); // 用于 Markdown 生成
```

### 注册 Swagger 服务

同样在 `Startup.cs` 文件中，配置 Swagger 服务：

```csharp
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Swagger API 文档", 
        Version = "v1", 
        Description = "API 文档" 
    });
    
    // 添加枚举过滤器，以在文档中显示枚举的描述信息
    c.DocumentFilter<SwaggerEnumFilter>(new object[]
    {
        // 枚举所在的程序集
        new[] { Assembly.GetExecutingAssembly() }
    });

    // 引入 XML 注释
    c.IncludeXmlComments("Samples.xml");
});
```

### 引用 Swagger 中间件

在 `Startup.cs` 文件中，添加 Swagger 中间件：

```csharp
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Samples v1"));
```

## 3. 生成 Markdown 文档

通过访问以下 URL 生成 Markdown 文档：

```
https://{localhost}:{port}/doc?swaggerVersion={swaggerVersion}
```

说明：`swaggerVersion` 是 Swagger 文档版本（在 `AddSwaggerGen` 中的 Version 参数，默认值为 v1）。

## 4. 生成示例

本示例使用 [Typora](https://www.typora.io/) 编辑器，并且下载 [Pandoc](https://github.com/jgm/pandoc/releases) 插件可以实现 Markdown 格式转换为 PDF 功能（免费）。如果需要样式调整，可以访问 [Typora 主题](https://theme.typora.io/) 进行选择。

### 示例展示

**Swagger 文档预览**

![Swagger API 文档预览](https://github.com/lwc1st/SwaggerDoc/blob/master/Docs/Images/api.png?raw=true)

**离线 PDF 文档预览**

![离线 PDF 文档预览](https://github.com/lwc1st/SwaggerDoc/blob/master/Docs/Images/swagger.png?raw=true)
