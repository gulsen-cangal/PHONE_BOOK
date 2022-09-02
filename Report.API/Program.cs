using Microsoft.EntityFrameworkCore;
using Report.API.Constants;
using Report.API.Entities.Context;
using Report.API.Middleware;
using Report.API.ServiceExtension;
using Report.API.Services;
using Report.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ReportContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("ConString")));
builder.Services.Configure<ReportSettings>(builder.Configuration.GetSection("Options"));
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();
app.Services.CreateScope().ServiceProvider.GetRequiredService<ReportContext>().Database.Migrate();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRabbitMq();

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
