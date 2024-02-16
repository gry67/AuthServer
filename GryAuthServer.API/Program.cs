using FluentValidation.AspNetCore;
using GryAuthServer.Core.Configuration;
using GryAuthServer.Core.Model;
using GryAuthServer.Core.Repository;
using GryAuthServer.Core.Services;
using GryAuthServer.Core.UnitOfWork;
using GryAuthServer.Data;
using GryAuthServer.Data.Repositories;
using GryAuthServer.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using SharedLibrary.Extensions;
using SharedLibrary.Exceptions;


var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration; // Alt satýrdaki Configuration kelimesi kullanýlabilmesi için eklendi

// Add services to the container.

//DI Register

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IServiceGeneric<,>), typeof(ServiceGeneric<,>));
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("GryAuthServer.Data");
    });
});

builder.Services.AddIdentity<UserApp, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<CustomTokenOption>(Configuration.GetSection("TokenOption")); //Option Pattern
builder.Services.Configure<List<Client>>(Configuration.GetSection("Clients")); //option pattern


builder.Services.AddAuthentication(options =>
{
    //jwt kimlik doðrulama þemasýný ayarla
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    //Token seçeneklerini appsettings.json dan al
    var tokenOptions = Configuration.GetSection("TokenOption").Get<CustomTokenOption>();

    // Token doðrulama parametrelerini ayarla
    opts.TokenValidationParameters = new TokenValidationParameters()
    {
        // Token'in geçerli olduðu yayýncý (issuer)
        ValidIssuer = tokenOptions.Issuer,
        // Token'in geçerli olduðu hedef domainler (audience)
        ValidAudience = tokenOptions.Audience[0],
        // Token'in imzalanmýþ olmasý için kullanýlan gizli anahtarý
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

        // Ýmzalama anahtarýný doðrula
        ValidateIssuerSigningKey = true,
        // Token'in hedef domain'i doðrula
        ValidateAudience = true,
        // Token'in yayýncýyý doðrula
        ValidateIssuer = true,
        // Token'in ömrünü doðrula
        ValidateLifetime = true,
        // tokenin ömrünü fazladan uzatmasýný engelledik.
        ClockSkew = TimeSpan.Zero 
    };
});


builder.Services.AddControllers().AddFluentValidation(options =>
{
    //Program.cs dosyasýnýn bulunduðu assembly deki tüm AbstractValidator sýnýfýndan miras alan sýnýflarý bul.
    options.RegisterValidatorsFromAssemblyContaining<Program>();
});

// Extension ekledik.
builder.Services.UseCustomValidationResponse();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCustomException();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
