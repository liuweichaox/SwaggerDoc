# 简介

SwaggerDoc 是基于 Swashbuckle.AspNetCore 类库的离线文档生工具。文档以 JSON 结构描述参数说明，支持枚举类型描述。工具导出 Markdown 格式文件，可以根据自己需求再将 Markdown 文件转换为自己所需要的文件格式。

##  1、SwaggerDoc引用 

### 主要接口

```C#
public interface ISwaggerDocGenerator
{
    Task<MemoryStream> GetSwaggerDocStreamAsync(string name);
    string GetSwaggerDoc(string name);
}
```
## 2、Startup配置

### 注册SwaggerDoc服务

```C#
services.AddSwaggerDoc();//（用于MarkDown生成）
```

### 注册Swagger服务

```C#
services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger API 示例文档", Version = "v1",Description="API文档全部由代码自动生成" });
	c.IncludeXmlComments("Samples.xml");
});
```

### 引用Swagger中间件

```C#
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Samples v1"));
```
## 3、生成MarkDown

```C#
/// <summary>
/// SwaggerController
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class SwaggerController : ControllerBase
{
    /// <summary>
    /// API文档导出
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> SwaggerDoc([FromServices] ISwaggerDocGenerator swaggerDocGenerator, [FromServices] IWebHostEnvironment environment)
    {
        var stream = await swaggerDocGenerator.GetSwaggerDocStreamAsync("v1");
        var mime = "application/octet-stream";
        var name = "SwaggerDoc.md";
        return File(stream.ToArray(), mime,name);
    }
}
```
## 4、生成示例

这里用的是 [typora](https://www.typora.io/) 编辑器，下载 [pandoc](https://github.com/jgm/pandoc/releases) 插件可以实现Marddown格式转换为PDF功能（免费）

如果需要样式调整，可以去https://theme.typora.io/ 选



**Swagger文档**

![api.png](https://github.com/lwc1st/SwaggerDoc/blob/master/Docs/Images/api.png?raw=true)

**离线PDF文档**

![swagger.png](https://github.com/lwc1st/SwaggerDoc/blob/master/Docs/Images/swagger.png?raw=true)

