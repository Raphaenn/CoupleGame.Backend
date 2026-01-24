using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Api.Extensions;
using Infrastructure.Data.Connections;
using Microsoft.IdentityModel.JsonWebTokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Segurança no Swagger (Bearer)
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Informe: Bearer {seu-token}"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// === JWT Bearer ===
var issuer   = builder.Configuration["Jwt:Issuer"];   // ex.: https://auth.seudominio.com
var audience = builder.Configuration["Jwt:Audience"]; // ex.: friends-api
var secret   = builder.Configuration["Jwt:Secret"];   // mesma chave da API 1 (se HS256)

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.MapInboundClaims = false;

        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),

            NameClaimType = JwtRegisteredClaimNames.Sub, // "sub"
            RoleClaimType = "role"
        };

        o.IncludeErrorDetails = true; // dev
    });



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
builder.Services.AddQuizServices();
builder.Services.AddAnswerServices();
builder.Services.AddInviteServices();
builder.Services.AddRecommendationServices();
builder.Services.AddInteractionServices();

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

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();