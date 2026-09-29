using plc;
using plc.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/health", () => "ok");

app.Run();
