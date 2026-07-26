using Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ConexaoSolidaria.API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "Jwt",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT gerado no login."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


#region DI
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICampanhaService, CampanhaService>();
builder.Services.AddScoped<IDoacaoService, DoacaoService>();
#endregion

#region JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSettings = jwtSection.Get<JwtSettings>();
Console.WriteLine(typeof(JwtSecurityTokenHandler).Assembly.FullName);
Console.WriteLine(typeof(TokenValidationParameters).Assembly.FullName);
builder.Services.Configure<JwtSettings>(jwtSection);

var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true
    };
});
#endregion

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GestorPolicy", policy => policy.RequireRole("GestorONG"));
    options.AddPolicy("DoadorPolicy", policy => policy.RequireRole("Doador", "GestorONG"));
});

#region RabbitMq
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMQ:Host"]
            ?? throw new InvalidOperationException(
                "RabbitMQ:Host não configurado.");

        var username = builder.Configuration["RabbitMQ:Username"]
            ?? throw new InvalidOperationException(
                "RabbitMQ:Username não configurado.");

        var password = builder.Configuration["RabbitMQ:Password"]
            ?? throw new InvalidOperationException(
                "RabbitMQ:Password não configurado.");

        var port = builder.Configuration
            .GetValue<ushort?>("RabbitMQ:Port") ?? 5672;

        cfg.Host(host, port, "/", h =>
        {
            h.Username(username);
            h.Password(password);
        });
    });
});
#endregion

var app = builder.Build();

// Aplica migrations automaticamente ao iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao aplicar migrations: {ex.Message}");
    }
}


app.MapGet("/", () => "API OK");
//app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
