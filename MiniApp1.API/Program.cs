using Microsoft.Extensions.Configuration;
using SharedLibrary.Configurations;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var Configuration = builder.Configuration; // Alt satýrdaki Configuration kelimesi kullanýlabilmesi için eklendi

builder.Services.Configure<CustomTokenOption>(Configuration.GetSection("TokenOption")); //Option Pattern
var tokenOptions = Configuration.GetSection("TokenOption").Get<CustomTokenOption>();

builder.Services.AddCustomTokenAuth(tokenOptions);

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
