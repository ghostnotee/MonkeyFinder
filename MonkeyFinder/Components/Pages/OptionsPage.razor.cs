using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Color = Microsoft.Maui.Graphics.Color;

namespace MonkeyFinder.Components.Pages;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class OptionsPage(IJSRuntime jsRuntime)
{
    private string _selectedTheme = string.Empty;
    public DesignThemeModes Mode { get; set; } = DesignThemeModes.Light;

    private async Task CheckInternet()
    {
        var hasInternet = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        var internetType = Connectivity.Current.ConnectionProfiles.FirstOrDefault();

        await ((Application)App).Windows[0].Page!.DisplayAlert("Connectivity Status",
            $"Internet access: {hasInternet} Internet Type: {internetType}", "OK");
    }

    public async Task HandleOnMenuChanged(MenuChangeEventArgs args)
    {
        _selectedTheme = args.Id ?? "1";
        Debug.WriteLine($"Selected theme: {_selectedTheme}");
        var selectedValue = args.Id;

        if (string.Equals(selectedValue, "2", StringComparison.OrdinalIgnoreCase))
        {
            Mode = DesignThemeModes.Dark;
            await jsRuntime.InvokeVoidAsync("setTheme", "dark");
            ((Application)App).UserAppTheme = AppTheme.Dark;
            
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                StatusBar.SetColor(Color.FromArgb("#444034"));
                StatusBar.SetStyle(StatusBarStyle.LightContent);
            }
        }
        else if (string.Equals(selectedValue, "1", StringComparison.OrdinalIgnoreCase))
        {
            Mode = DesignThemeModes.Light;
            await jsRuntime.InvokeVoidAsync("setTheme", "light");
            ((Application)App).UserAppTheme = AppTheme.Dark;
            
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                StatusBar.SetColor(Color.FromArgb("#DDAF24"));
                StatusBar.SetStyle(StatusBarStyle.DarkContent);
            }
        }
        else
        {
            Mode = DesignThemeModes.System;
            var currentTheme = ((Application)App).RequestedTheme;
            if (currentTheme == AppTheme.Dark)
            {
                await jsRuntime.InvokeVoidAsync("setTheme", "dark");
                
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    StatusBar.SetColor(Color.FromArgb("#444034"));
                    StatusBar.SetStyle(StatusBarStyle.LightContent);
                }
            }
            else
            {
                await jsRuntime.InvokeVoidAsync("setTheme", "light");
                
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    StatusBar.SetColor(Color.FromArgb("#DDAF24"));
                    StatusBar.SetStyle(StatusBarStyle.DarkContent);
                }
            }
        }
    }
}