namespace MonkeyFinderHybrid;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new NavigationPage(new MainPage())
		{
			BarBackgroundColor = Color.FromArgb("#FFC107"),
			BarTextColor = Colors.White
		});
	}
}
