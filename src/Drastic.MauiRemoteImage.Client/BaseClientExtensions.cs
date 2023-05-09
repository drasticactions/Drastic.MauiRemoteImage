using System;
using System.Threading.Tasks;
using Drastic.MauiRemoteImage.Messages;

namespace Drastic.MauiRemoteImage.Client;

public static class BaseClientExtensions
{
    public static Task SendMessageAsync(this AppClient client, DiagnosticMessage message)
    {
        ArgumentNullException.ThrowIfNull(nameof(message));

        message.Id = client.Id;
        return client.Connection.SendAsync(message);
    }
}