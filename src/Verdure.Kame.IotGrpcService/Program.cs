using Verdure.Kame.Core;
using Verdure.Kame.IotGrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc(option =>
{
    option.MaxReceiveMessageSize = null;
});
builder.Services.AddSingleton<IQuadruped, Quadruped>();
builder.Services.AddSingleton<IQuadrupedFaceScreen, QuadrupedFaceScreen>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DataTransmissionGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
