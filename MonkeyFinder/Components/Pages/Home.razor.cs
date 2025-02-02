using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using MonkeyFinder.Components.Controls;
using MonkeyFinder.Model;
using MonkeyFinder.Services;
using MonkeyFinder.Services.Permission;

namespace MonkeyFinder.Components.Pages;

public partial class Home(
    MonkeyService monkeyService,
    NavigationManager navigationManager,
    IDialogService dialogService,
    IGeolocation geolocation,
    IPermissionService permissionService)
{
    private List<Monkey> _monkeys = [];
    private Monkey DialogData { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        _monkeys = await monkeyService.GetMonkeysAsync();
    }

    private async Task AddMonkey()
    {
        // MAUI Debug console
        // Debug.WriteLine("Add Monkey");

        // Create a new instance of DialogData to allow the user to cancel the update
        var data = new Monkey();
        var dialog = await dialogService.ShowDialogAsync<SimpleCustomizedDialog>(data, new DialogParameters
        {
            Title = "Add a New Monkey",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true
        });

        var result = await dialog.Result;
        if (result is { Cancelled: false, Data: not null })
        {
            DialogData = (Monkey)result.Data;
            monkeyService.AddMonkey(DialogData);
            _monkeys = await monkeyService.GetMonkeysAsync();
        }
    }

    private void GoToDetails(Monkey monkey)
    {
        navigationManager.NavigateTo($"/details/{monkey.Name}");
    }

    private async Task FindMonkey()
    {
        var status = await permissionService.CheckAndRequestPermissionAsync<Permissions.LocationWhenInUse>();
        if (status == PermissionStatus.Granted)
        {
            try
            {
                // Get cached location, else get real location.
                var location = await geolocation.GetLastKnownLocationAsync() ?? await geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });

                // Find closest monkey to us
                var closestMonkey = _monkeys.OrderBy(m => location.CalculateDistance(
                        new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
                    .FirstOrDefault();

                var closestMonkeyMessage = string.Empty;

                closestMonkeyMessage = closestMonkey is not null
                    ? $"{closestMonkey.Name} is closest, this monkey is in {closestMonkey.Location}"
                    : "The closest monkey could not be determined!";

                await ((Application)App).Windows[0].Page!.DisplayAlert("Closest Monkey",
                    closestMonkeyMessage, "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to query location: {ex.Message}");
                await ((Application)App).Windows[0].Page!.DisplayAlert("Error!", ex.Message, "OK");
            }
        }
        else
        {
            Console.WriteLine("❌ Kamera izni reddedildi!");
        }
    }
}