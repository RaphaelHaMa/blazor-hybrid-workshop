using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using MonkeyFinderHybrid.Components.Controls;

namespace MonkeyFinderHybrid.Components.Pages
{
    public partial class Home
    {
        private List<Monkey> monkeys = new();

        [Inject]
        public MonkeyService MonkeyService { get; set; } = default!;

        [Inject]
        public IDialogService DialogService { get; set; } = default!;

        [Inject]
        public NavigationManager NavManager { get; set; } = default!;

        [Inject]
        public IGeolocation Geolocation { get; set; } = default!;

        private Monkey DialogData { get; set; } = new();        

        protected override async Task OnInitializedAsync()
        {
            monkeys = await MonkeyService.GetMonkeysAsync();
        }

        private async Task AddMonkey()
        {
            var data = new Monkey();
            var dialog = await DialogService.ShowDialogAsync<SimpleCustomizedDialog>(data, new DialogParameters()
            {
                Title = $"Add a New Monkey",
                PreventDismissOnOverlayClick = true,
                PreventScroll = true
            });

            var result = await dialog.Result;
            if (!result.Cancelled && result.Data is not null)
            {
                DialogData = (Monkey)result.Data;
                monkeys = MonkeyService.AddMonkey(DialogData);

            }
        }

        private void GoToDetails(Monkey monkey)
        {
            NavManager.NavigateTo($"details/{monkey.Name}");
        }

        private async Task FindMonkey()
        {
            try
            {
                // Get cached location, else get real location.
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location is null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                // Find closest monkey to us
                var closestMonkey = monkeys.OrderBy(m => location.CalculateDistance(
                    new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
                    .FirstOrDefault();

                var closestMonkeyMessage = string.Empty;

                if (closestMonkey is not null)
                {
                    closestMonkeyMessage = $"{closestMonkey.Name} is closest, this monkey is in {closestMonkey.Location}";
                }
                else
                {
                    closestMonkeyMessage = "The closest monkey could not be determined!";
                }

                await ((Application)app).Windows[0].Page!.DisplayAlert("Closest Monkey",
                    closestMonkeyMessage, "OK");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to query location: {ex.Message}");
                await ((Application)app).Windows[0].Page!.DisplayAlert("Error!", ex.Message, "OK");
            }
        }
    }
}
