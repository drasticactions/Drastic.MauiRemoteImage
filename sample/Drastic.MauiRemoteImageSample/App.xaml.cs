using Drastic.MauiRemoteImage.Client;
using Drastic.Tools;
using Microsoft.Extensions.Logging;

namespace Drastic.MauiRemoteImageSample;

public partial class App : Application
{
	private AppClient client;
	
	public App(IServiceProvider provider)
	{
		InitializeComponent();
		var logger = provider.GetService<ILogger>();
		var ip = "";
		if (string.IsNullOrEmpty(ip))
		{
			throw new ArgumentNullException("You must set the IP Address for the client to connect!");
		}
		this.client = new AppClient("TestClient", logger);
		this.client.ConnectAsync(new Drastic.Tempest.Target(ip, 8888)).FireAndForgetSafeAsync();
		MainPage = new AppShell();
	}
}
