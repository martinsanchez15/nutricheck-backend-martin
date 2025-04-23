using NutriCheck.Models;
using NutriCheck.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// 🔐 Clave secreta para JWT
var key = Encoding.UTF8.GetBytes("nutricheck-superclave-segura-2025!!");

// Leer configuración de MongoDB
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDbService>();

// JWT - Configuración de autenticación
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// 📄 Licencia QuestPDF
QuestPDF.Settings.License = LicenseType.Community;

// Servicios para trabajar con la base de datos en memoria (si es necesario, puedes cambiar a MongoDB completamente más tarde)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("NutriCheckDb"));

// Configurar los controladores y las opciones de JSON
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();

// 📚 Swagger + JWT
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Ingresa tu token JWT como: Bearer {tu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Precarga de nutricionista de prueba en MongoDB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var mongoService = scope.ServiceProvider.GetRequiredService<MongoDbService>();

    // Si la base de datos está vacía, agrega un nutricionista de prueba
    if (!db.Nutricionistas.Any())
    {
        db.Nutricionistas.Add(new Nutricionista
        {
            Nombre = "Martin Sanchez",
            Email = "martin@nutricheck.com",
            Password = "1234",
            Rol = "Nutricionista"
        });

        db.SaveChanges();
    }

    // Si es necesario precargar MongoDB, agrega datos de prueba en MongoDB
    if (!mongoService.HasPreferences())
    {
        mongoService.AddPreferences(new Preferences
        {
            // Datos de prueba en MongoDB
        });
    }
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();