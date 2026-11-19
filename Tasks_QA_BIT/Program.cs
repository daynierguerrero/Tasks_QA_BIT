using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Tasks_QA_BIT.Application;
using Tasks_QA_BIT.Infrastructure;
using Tasks_QA_BIT.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.Run();

public partial class Program { }
