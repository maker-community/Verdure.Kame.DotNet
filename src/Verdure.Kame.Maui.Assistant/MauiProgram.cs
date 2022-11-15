using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Verdure.Kame.Core.Services;
using Verdure.Kame.DataTransmission;

namespace Verdure.Kame.Maui.Assistant
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddGrpcClient<DataTransmissionGrpc.DataTransmissionGrpcClient>(o =>
            {
                //o.Address = new Uri("http://192.168.3.239:5241");
                o.Address = new Uri("http://192.168.3.221:5241");
            });

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddSingleton<DataTransmissionClient>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}