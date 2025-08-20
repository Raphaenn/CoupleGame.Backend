using Api.Extensions;
using Infrastructure.Data.Connections;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Singleton para a fonte de dados (DataSource é thread-safe e recomendado como singleton)
builder.Services.AddSingleton<PostgresConnection>(_ =>
{
    string? connectionString = builder.Configuration.GetConnectionString(name: "DefaultConnection");
    return new PostgresConnection(connectionString!);
});

builder.Services.AddScoped<DbSession>();
builder.Services.AddUserService();
builder.Services.AddCoupleServices();
builder.Services.AddTopicService();
builder.Services.AddQuestionServices();
builder.Services.AddQuestionServices();
builder.Services.AddQuizServices();
builder.Services.AddAnswerServices();
builder.Services.AddInviteServices();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();