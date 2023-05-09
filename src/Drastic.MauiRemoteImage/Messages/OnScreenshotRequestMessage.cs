using Drastic.Tempest;

namespace Drastic.MauiRemoteImage.Messages;

public class OnScreenshotRequestMessage
    : DiagnosticMessage
{

    public OnScreenshotRequestMessage()
        : base(DiagnosticsMessageType.OnScreenshotRequest)
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