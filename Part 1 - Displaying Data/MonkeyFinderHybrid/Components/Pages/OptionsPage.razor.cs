namespace MonkeyFinderHybrid.Components.Pages
{
    public partial class OptionsPage
    {
        private async Task CheckInternet()
        {
            var hasInternet = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            var internetType = Connectivity.Current.ConnectionProfiles.FirstOrDefault();

            await ((Application)app).Windows[0].Page!.DisplayAlert("Connectivity Status", $"Internet access: {hasInternet} of type {internetType}", "OK");
        }
    }
}
