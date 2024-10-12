using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;

namespace MonkeyFinderHybrid.Components.Pages
{
    public partial class OptionsPage
    {
        [Inject]
        private IJSRuntime JS { get; set; }

        public DesignThemeModes Mode { get; set; } = DesignThemeModes.Light;

        public async Task CheckInternet()
        {
            var hasInternet = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            var internetType = Connectivity.Current.ConnectionProfiles.FirstOrDefault();

            await ((Application)app).Windows[0].Page!.DisplayAlert("Connectivity Status", $"Internet access: {hasInternet} of type {internetType}", "OK");
        }

        private string selectedTheme = string.Empty;

        public async Task HandleOnMenuChanged(MenuChangeEventArgs args)
        {
            selectedTheme = args.Id ?? "1";
            Debug.WriteLine($"Selected Value: {args.Id}");
            var selectedValue = args.Id;

            if (string.Equals(selectedValue, "2"))
            {
                Mode = DesignThemeModes.Dark;
                await JS.InvokeVoidAsync("setTheme", "dark");
                ((Application)app).UserAppTheme = AppTheme.Dark; // This was added, we will get to it in a minute

                // This whole block was added
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    StatusBar.SetColor(Microsoft.Maui.Graphics.Color.FromArgb("#444034"));
                    StatusBar.SetStyle(StatusBarStyle.LightContent);
                }
            }
            else if (string.Equals(selectedValue, "1"))
            {
                Mode = DesignThemeModes.Light;
                await JS.InvokeVoidAsync("setTheme", "light");
                ((Application)app).UserAppTheme = AppTheme.Light; // This was added, we will get to it in a minute

                // This whole block was added
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    StatusBar.SetColor(Microsoft.Maui.Graphics.Color.FromArgb("#DDAF24"));
                    StatusBar.SetStyle(StatusBarStyle.DarkContent);
                }
            }
            else
            {
                Mode = DesignThemeModes.System;
                AppTheme currentTheme = ((Application)app).RequestedTheme;
                Debug.WriteLine("Current System Theme : " + currentTheme.ToString());

                if (currentTheme == AppTheme.Dark)
                {
                    await JS.InvokeVoidAsync("setTheme", "dark");

                    // This whole block was added
                    if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        StatusBar.SetColor(Microsoft.Maui.Graphics.Color.FromArgb("#444034"));
                        StatusBar.SetStyle(StatusBarStyle.LightContent);
                    }
                }
                else
                {
                    await JS.InvokeVoidAsync("setTheme", "light");

                    // This whole block was added
                    if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        StatusBar.SetColor(Microsoft.Maui.Graphics.Color.FromArgb("#DDAF24"));
                        StatusBar.SetStyle(StatusBarStyle.DarkContent);
                    }
                }
            }
        }
    }
}
