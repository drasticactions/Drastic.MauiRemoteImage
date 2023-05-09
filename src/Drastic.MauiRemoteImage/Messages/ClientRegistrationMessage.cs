using Drastic.Tempest;

namespace Drastic.MauiRemoteImage.Messages;

public class ClientRegistrationMessage
    : DiagnosticMessage
{
    public ClientRegistrationMessage()
        : base(DiagnosticsMessageType.ClientRegistration)
    {
    }

    /// <inheritdoc/>
    public override void ReadPayload(ISerializationContext context, IValueReader reader)
    {
        base.ReadPayload(context, reader);
    }

    /// <inheritdoc/>
    public override void WritePayload(ISerializationContext context, IValueWriter writer)
    {
        base.WritePayload(context, writer);
    }
}