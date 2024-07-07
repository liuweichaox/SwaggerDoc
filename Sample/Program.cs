using System.Reflection;
using Microsoft.OpenApi.Models;
using SwaggerDoc;
using SwaggerDoc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "Swagger API 文档", Version = "v1", Description = "API 文档"});
    // 添加枚举过滤器，在文档中显示枚举的描述信息
    c.DocumentFilter<SwaggerEnumFilter>(new object[]
    {
        // 枚举所在的程序集
        new[] {Assembly.GetExecutingAssembly()}
    });
    c.IncludeXmlComments("Sample.xml");
});
builder.Services.AddSwaggerDoc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();