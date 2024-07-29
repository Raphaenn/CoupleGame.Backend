using Api.Extensions;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Data.Connections;
using Infrastructure.Repository.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<PostgresConnection>(_ =>
{
    string? connectionString = builder.Configuration.GetConnectionString(name: "DefaultConnection");
    return new PostgresConnection(connectionString);
});

builder.Services.AddUserService();
builder.Services.AddCoupleServices();
builder.Services.AddTopicService();
builder.Services.AddQuestionServices();

var app = builder.Build();

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