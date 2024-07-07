# 简介

SwaggerDoc 是基于 Swashbuckle.AspNetCore 类库的离线文档生工具。文档以 JSON 结构描述参数说明，支持枚举类型描述。工具导出 Markdown 格式文件，可以根据自己需求再将 Markdown 文件转换为自己所需要的文件格式。

项目地址：https://github.com/liuweichaox/SwaggerDoc

##  1、SwaggerDoc引用 

### .NET CLI

```
dotnet add package SwaggerDoc --version 1.0.1
```
## 2、Startup配置

### 注册 SwaggerDoc 服务

```C#
services.AddSwaggerDoc();//（用于MarkDown生成）
```

### 注册 Swagger 服务

笔记：项目属性配置允许 **XML** 生成

```C#
services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo {Title = "Swagger API 文档", Version = "v1", Description = "API 文档"});
	// 添加枚举过滤器，在文档中显示枚举的描述信息
	c.DocumentFilter<SwaggerEnumFilter>(new object[]
	{
		// 枚举所在的程序集
		new[] {Assembly.GetExecutingAssembly()}
	});
	c.IncludeXmlComments("Samples.xml");
});
```

### 引用 Swagger中间件

```C#
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Samples v1"));
```

## 3、生成MarkDown

Get 访问 https://{localhost}:{port}/doc?swaggerVersion={swaggerVersion}

说明：swaggerVersion 是 swagger 文档版本（AddSwaggerGen 中的 Version 参数，默认 v1）

## 4、生成示例

这里用的是 [typora](https://www.typora.io/) 编辑器，下载 [pandoc](https://github.com/jgm/pandoc/releases) 插件可以实现Marddown格式转换为PDF功能（免费）

如果需要样式调整，可以去https://theme.typora.io/ 选



**Swagger文档**

![api.png](https://github.com/lwc1st/SwaggerDoc/blob/master/Docs/Images/api.png?raw=true)

**离线PDF文档**

![swagger.png](https://github.com/lwc1st/SwaggerDoc/blob/master/Docs/Images/swagger.png?raw=true)

