using Drastic.MauiRemoteImage.Client;
using Drastic.MauiRemoteImage.Messages;
using Drastic.Tools;

namespace Drastic.MauiRemoteImageSample;

public partial class MainPage : ContentPage
{
	int count = 0;
	private AppClient client;
	
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

