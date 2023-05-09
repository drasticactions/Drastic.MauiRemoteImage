using System;
using System.Linq;
using System.Threading.Tasks;
using Drastic.MauiRemoteImage.Messages;
using Drastic.MauiRemoteImage.Models;
using Drastic.Tempest;
using Drastic.Tempest.Providers.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Dispatching;

namespace Drastic.MauiRemoteImage.Client;

public class AppClient 
    : TempestClient
{
    private readonly ILogger? logger;
    private Guid id;
    private string name;

    public AppClient(string name = "", ILogger? logger = default)
        : this(DiagnosticsProtocol.Instance, name, logger)
    {
        this.Connected += this.AppClient_Connected;
    }
    
    public AppClient(Protocol protocol, string name = "", ILogger? logger = default)
        : base(new NetworkClientConnection(protocol), MessageTypes.Reliable)
    {
        this.name = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString() : name;
        this.id = Guid.NewGuid();
        this.logger = logger;
        this.Connected += this.DiagnosticsClient_Connected;
        this.Disconnected += this.DiagnosticsClient_Disconnected;
        this.RegisterMessageHandler<TestRequestMessage>(this.OnTestRequest);
        this.RegisterMessageHandler<TestResponseMessage>(this.OnTestResponse);
        this.RegisterMessageHandler<OnScreenshotRequestMessage>(this.OnScreenshotRequest);
    }

    private async void OnScreenshotRequest(MessageEventArgs<OnScreenshotRequestMessage> obj)
    {
        await this.SendScreenshots();
    }

    public string Id => $"{this.name}-{this.id}";
    
    internal ILogger? Logger => this.logger;

    private void OnTestResponse(MessageEventArgs<TestResponseMessage> args)
    {
        this.logger?.LogInformation(args.Message.ToString());
    }

    private void OnTestRequest(MessageEventArgs<TestRequestMessage> args)
    {
        this.logger?.LogInformation(args.Message.ToString());
        this.SendMessageAsync(new TestResponseMessage());
    }

    private void DiagnosticsClient_Disconnected(object? sender, ClientDisconnectedEventArgs e)
    {
        this.logger?.LogInformation($"Disconnect: {e.Reason}");
    }

    private void DiagnosticsClient_Connected(object? sender, ClientConnectionEventArgs e)
    {
        this.logger?.LogInformation($"Connect");
    }

    public async Task SendScreenshots()
    {
        var app = Microsoft.Maui.Controls.Application.Current;
        if (app is null)
        {
            return;
        }

        await app.Dispatcher.DispatchAsync(async () =>
        {
            var screenshots = await Task.WhenAll(app.Windows.Select(n => VisualDiagnostics.CaptureAsPngAsync(n)!));
            var fun = screenshots.Select(n => new Screenshot() { Name = Guid.NewGuid().ToString(), Image = n });
            var message = new OnScreenshotResponseMessage() { ScreenShots = fun};
            await this.SendMessageAsync(message);
        });
    }
    
    private void AppClient_Connected(object? sender, ClientConnectionEventArgs e)
    {
        // Tell the server our ID.
        this.SendMessageAsync(new ClientRegistrationMessage());
    }
}