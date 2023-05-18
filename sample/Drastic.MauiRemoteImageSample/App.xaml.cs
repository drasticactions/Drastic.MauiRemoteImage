using Drastic.MauiRemoteImage.Client;
using Drastic.Tempest;
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
		Task.Run(async () =>
		{
			
			var ips = Drastic.LocalNetworkAddresses.RemoteNetworkAddresses.GetLocalMachineAddresses();
			this.client = new AppClient("TestClient", logger);
			var isConnected = false;
			foreach (var ip in ips)
			{
				try
				{
					var result = await this.client.ConnectAsync(new Target(ip, 8888));
					if (result.Result == ConnectionResult.Success)
					{
						isConnected = true;
						break;
					}
				}
				catch (Exception e)
				{
					System.Diagnostics.Debug.WriteLine(e);
					throw;
				}
			}

			if (!isConnected)
			{
				throw new ArgumentException("Could not connect to the server. Is it running?");
			}
		}).FireAndForgetSafeAsync();
		MainPage = new AppShell();
	}
}
