using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuovoCinemaParadiso.Data;
using NuovoCinemaParadiso.Helpers;
using NuovoCinemaParadiso.Models;
using NuovoCinemaParadiso.Services;
using NuovoCinemaParadiso.Seed;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddControllers();

builder.Services.AddDbContext<ContestoDb>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityCore<Utente>(options =>
{
    
    options.Password.RequireDigit = false; 
    options.Password.RequireLowercase = false; 
    options.Password.RequireUppercase = false; 
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequiredLength = 6; 
})
.AddRoles<IdentityRole>() // <-- fondamentale per i ruoli
.AddSignInManager<SignInManager<Utente>>()
.AddEntityFrameworkStores<ContestoDb>()
.AddDefaultTokenProviders();

string? jwtKey = builder.Configuration["Jwt:Key"];
string? jwtIssuer = builder.Configuration["Jwt:Issuer"];
string? jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey) ||
    string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new Exception("Configurazione JWT mancante in appsettings.json");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Permette ad Angular locale di chiamare l'API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<GenereMovieService>();
builder.Services.AddScoped<TipologiaSalaService>();
builder.Services.AddScoped<TurnoService>();
builder.Services.AddScoped<SalaService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<AcquistoService>();
builder.Services.AddScoped<RuoloUtenteService>(); // <-- nuovo servizio per gestire i ruoli degli utenti
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<LogAzioniService>();
builder.Services.AddScoped<UtenteService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AbbonamentoService>();
builder.Services.AddScoped<ProiezioneService>();


var app = builder.Build();

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContestoDb>();
    db.Database.Migrate();
}

//seed ruoli + utenti + interessi
await DataSeeder.SeedAsync(app.Services);

app.Run();