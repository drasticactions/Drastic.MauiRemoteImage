using Drastic.MauiRemoteImage.Models;
using Drastic.Tempest;

namespace Drastic.MauiRemoteImage.Messages;

public class OnScreenshotResponseMessage
    : DiagnosticMessage
{

    public OnScreenshotResponseMessage()
        : base(DiagnosticsMessageType.OnScreenshotResponse)
    {
    }
    
    public IEnumerable<Screenshot> ScreenShots { get; internal set; }

    /// <inheritdoc/>
    public override void ReadPayload(ISerializationContext context, IValueReader reader)
    {
        this.ScreenShots = reader.ReadEnumerable<Screenshot>(context, new ScreenshotSerializer());
        base.ReadPayload(context, reader);
    }

    /// <inheritdoc/>
    public override void WritePayload(ISerializationContext context, IValueWriter writer)
    {
        writer.WriteEnumerable<Screenshot>(context, this.ScreenShots);
        base.WritePayload(context, writer);
    }
    
    public class ScreenshotSerializer : ISerializer<Screenshot>
    {
        public Screenshot Deserialize(ISerializationContext context, IValueReader reader)
        {
            var screenshot = new Screenshot();
            screenshot.Name = reader.ReadString();
            screenshot.Image = reader.ReadBytes();
            return screenshot;
        }

        public void Serialize(ISerializationContext context, IValueWriter writer, Screenshot element)
        {
            writer.WriteString(element.Name);
            writer.WriteBytes(element.Image);
        }
    }
}