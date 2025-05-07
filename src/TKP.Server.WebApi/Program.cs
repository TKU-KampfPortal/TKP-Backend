using TKP.Server.Application;
using TKP.Server.Infrastructure;
using TKP.Server.Infrastructure.Data;
using TKP.Server.WebApi;
using TKP.Server.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.AddApplicationDI().AddInfrastructureDI().AddWebApiDI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Migrate Database
using var scope = app.Services.CreateAsyncScope();
await AutomatedMigration.MigrateAsync(scope.ServiceProvider, app.Configuration);

app.UseHttpsRedirection();

app.UseRouting(); // Thêm UseRouting() để ánh xạ endpoint

app.UseCors("AllowSpecificOrigins"); // Đặt CORS trước Authentication/Authorization
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
