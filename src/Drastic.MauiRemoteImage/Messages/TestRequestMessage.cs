using Drastic.Tempest;

namespace Drastic.MauiRemoteImage.Messages;

public class TestRequestMessage
    : DiagnosticMessage
{

    public TestRequestMessage()
        : base(DiagnosticsMessageType.TestRequest)
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