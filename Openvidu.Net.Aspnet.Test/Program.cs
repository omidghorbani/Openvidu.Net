using Openvidu.Net.Aspnet.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureOpenVidu();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOpenVidu(new OpenViduWebhookOption()
{
    AcceptHeaders = new List<(string header, string value)>()
    {
        ("Authorization", "My_CUSTOM_TOKEN_FOR_VR"),
    }
});
app.UseAuthorization();

app.MapControllers();

app.Run();
