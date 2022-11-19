using Microsoft.Extensions.Logging;
using Verdure.Kame.Core.Services;
using Verdure.Kame.DataTransmission;
using Verdure.Kame.Maui.Assistant.Services;

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
                o.Address = new Uri("http://192.168.3.239:5241");
                //o.Address = new Uri("http://192.168.3.221:5241");
            });

            builder.Services.AddTransient<MainPage>();
#if WINDOWS
            builder.Services.AddTransient<IFaceScreenMediaPlayer, Verdure.Kame.Maui.Assistant.Platforms.Windows.FaceScreenMediaPlayer>();
#endif
            builder.Services.AddSingleton<DataTransmissionClient>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}