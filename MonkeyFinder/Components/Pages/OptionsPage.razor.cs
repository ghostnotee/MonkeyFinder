using System.Diagnostics;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MonkeyFinder.Components.Pages;

public partial class OptionsPage(IJSRuntime jsRuntime)
{
    private string _selectedTheme = string.Empty;

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
        
        if(string.Equals(selectedValue, "2", StringComparison.OrdinalIgnoreCase))
            await jsRuntime.InvokeVoidAsync("setTheme", "dark");
        else if(string.Equals(selectedValue, "1", StringComparison.OrdinalIgnoreCase))
            await jsRuntime.InvokeVoidAsync("setTheme", "light");
        else
        {
            var currentTheme = ((Application)App).RequestedTheme;
            if(currentTheme == AppTheme.Dark)
                await jsRuntime.InvokeVoidAsync("setTheme", "dark");
            else
                await jsRuntime.InvokeVoidAsync("setTheme", "light");
        }
    }
}