using Microsoft.EntityFrameworkCore;
using Praksa.BLL.Contracts.Helpers;
using Praksa.BLL.Contracts.Services;
using Praksa.BLL.Helpers;
using Praksa.BLL.Services;
using Praksa.DAL;
using Praksa.DAL.Contracts.Repositories;
using Praksa.DAL.Repositories;
using Praksa.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPlatformCredentialsService, PlatformCredentialsService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IPlatformCredentialsRepository, PlatformCredentialsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IEncryptionHelper, EncryptionHelper>();
builder.Services.AddScoped<IHashHelper, HashHelper>();


var app = builder.Build();

app.UseExceptionHandler("/error");

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
