using Openvidu.Net.Aspnet.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureOpenVidu();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOpenVidu();
app.UseAuthorization();

app.MapControllers();

app.Run();
