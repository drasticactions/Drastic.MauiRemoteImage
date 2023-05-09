using Drastic.MauiRemoteImage.Messages;
using Drastic.Tempest;
using Microsoft.Extensions.Logging;

namespace Drastic.MauiRemoteImage.Server;

public class DiagnosticsServer : TempestServer
{
    private readonly ILogger? logger;
    private string outputDirectory;
    
    public DiagnosticsServer(IConnectionProvider provider, ILogger? logger = default)
        : base(provider, MessageTypes.Reliable)
    {
        this.logger = logger;
        string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        this.outputDirectory = Path.Combine(currentDirectory, "output");
        Directory.CreateDirectory(this.outputDirectory);
        
        this.RegisterMessageHandler<TestRequestMessage>(this.OnTestRequestMessage);
        this.RegisterMessageHandler<TestResponseMessage>(this.OnTestResponseMessage);
        this.RegisterMessageHandler<ClientRegistrationMessage>(this.OnClientRegistration);
        this.RegisterMessageHandler<OnScreenshotResponseMessage>(this.OnScreenshotResponse);
    }

    public void SendScreenshotRequest()
    {
        this.SendToAll(new OnScreenshotRequestMessage());
    }

    private void OnScreenshotResponse(MessageEventArgs<OnScreenshotResponseMessage> obj)
    {
        foreach (var item in obj.Message.ScreenShots)
        {
            var filename = $"{obj.Message.Id}-{item.Name}.png";
            var output = Path.Combine(this.outputDirectory, filename);
            File.WriteAllBytes(output, item.Image);
            this.logger?.LogInformation($"Saved {output}");
        }
    }

    private readonly List<IConnection> connections = new List<IConnection>();
    
    private void OnTestResponseMessage(MessageEventArgs<TestResponseMessage> args)
    {
        this.logger?.LogInformation(args.Message.ToString());

        this.SendToAll(args.Message, args.Connection);
    }

    private void OnTestRequestMessage(MessageEventArgs<TestRequestMessage> args)
    {
        this.logger?.LogInformation(args.Message.ToString());

        this.SendToAll(args.Message, args.Connection);
    }
    
    private void SendToAll(DiagnosticMessage message, IConnection? ogSender = default)
    {
        lock (this.connections)
        {
            var list = this.connections.Where(n => n != ogSender);
            foreach (var connection in list) {
                connection.SendAsync(message);
            }
        }
    }
    
    /// <inheritdoc/>
    protected override void OnConnectionMade(object sender, ConnectionMadeEventArgs e)
    {
        lock (this.connections)
        {
            this.connections.Add(e.Connection);
        }

        this.logger?.LogInformation(e.ToString());
        base.OnConnectionMade(sender, e);
    }

    /// <inheritdoc/>
    protected override void OnConnectionDisconnected(object sender, DisconnectedEventArgs e)
    {
        lock (this.connections)
        {
            this.connections.Remove(e.Connection);
        }

        this.logger?.LogInformation($"Disconnect");
        base.OnConnectionDisconnected(sender, e);
    }
    
    private void OnClientRegistration(MessageEventArgs<ClientRegistrationMessage> args)
    {
        var clientMessage = args.Message;
        
        
        this.logger?.LogInformation($"Client ID {clientMessage.Id} registered");
    }
}