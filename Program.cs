using ChickenCoop.Services; // Replace 'YourNamespace' with the actual namespace containing IAzureCommunicationService
using ChickenCoop.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAzureCommunicationService, AzureCommunicationService>();
builder.Services.AddSingleton<IAcsService, AcsService>();
builder.Services.Configure<AcsSettings>(builder.Configuration.GetSection("ACS"));

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
