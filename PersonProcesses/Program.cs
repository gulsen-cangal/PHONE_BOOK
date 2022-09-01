using Microsoft.EntityFrameworkCore;
using PersonProcesses.API.Services.Interfaces;
using PersonProcesses.API.Services;
using PersonProcesses.Entities.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PersonProcessesContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("ConString")));
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IContactInformationService, ContactInformationSevice>();
builder.Services.AddHttpClient();
var app = builder.Build();
app.Services.CreateScope().ServiceProvider.GetRequiredService<PersonProcessesContext>().Database.Migrate();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
