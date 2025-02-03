using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using MonkeyFinder.Model;
using MonkeyFinder.Services;
using MonkeyFinder.Services.Permission;

namespace MonkeyFinder;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddFluentUIComponents();

        builder.Services.AddSingleton<IPermissionService, PermissionService>();
        builder.Services.AddSingleton<MonkeyService>();
        builder.Services.AddSingleton<IGeolocation>(_ => Geolocation.Default);
        builder.Services.AddSingleton<IMap>(_ => Map.Default);
        builder.Services.AddSingleton<RatingState>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}