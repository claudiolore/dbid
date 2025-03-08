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


//    "DefaultConnection": "server=localhost;port=3306;database=dibid;user=root;password="