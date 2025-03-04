using Microsoft.EntityFrameworkCore;
using Data;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gestione Immobili API", Version = "v1" });

    // Configurazione per il supporto JWT in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    // Aggiungi i commenti XML per documentazione su Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0))
    )
);

// Register services
builder.Services.AddScoped<IComplessoService, ComplessoService>();
builder.Services.AddScoped<IEdificioService, EdificioService>();
builder.Services.AddScoped<IUnitaImmobiliareService, UnitaImmobiliareService>();
builder.Services.AddScoped<IStruttureService, StruttureService>();
builder.Services.AddScoped<IInfissiService, InfissiService>();
builder.Services.AddScoped<IIdraulicoAdduzioneService, IdraulicoAdduzioneService>();
builder.Services.AddScoped<IScarichiIdriciFognariService, ScarichiIdriciFognariService>();
builder.Services.AddScoped<IImpiantiElettriciService, ImpiantiElettriciService>();
builder.Services.AddScoped<IImpiantoClimaAcsService, ImpiantoClimaAcsService>();
builder.Services.AddScoped<IAltriImpiantiService, AltriImpiantiService>();
builder.Services.AddScoped<IDocumentiGeneraliService, DocumentiGeneraliService>();
builder.Services.AddScoped<ISegnalazioneProblemaService, SegnalazioneProblemaService>();
builder.Services.AddScoped<ISyncService, SyncService>();

// Configurazione dell'autenticazione JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? "QuestaDovrebbeEssereUnaChiaveSegretaMoltoLungaEComplessa12345")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
