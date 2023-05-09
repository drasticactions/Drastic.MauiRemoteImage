using System.Collections.Generic;
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
    
    public IEnumerable<Drastic.MauiRemoteImage.Models.Screenshot> ScreenShots { get; internal set; }

    /// <inheritdoc/>
    public override void ReadPayload(ISerializationContext context, IValueReader reader)
    {
        this.ScreenShots = reader.ReadEnumerable<Drastic.MauiRemoteImage.Models.Screenshot>(context, new ScreenshotSerializer());
        base.ReadPayload(context, reader);
    }

    /// <inheritdoc/>
    public override void WritePayload(ISerializationContext context, IValueWriter writer)
    {
        writer.WriteEnumerable<Drastic.MauiRemoteImage.Models.Screenshot>(context, this.ScreenShots);
        base.WritePayload(context, writer);
    }
    
    public class ScreenshotSerializer : ISerializer<Drastic.MauiRemoteImage.Models.Screenshot>
    {
        public Drastic.MauiRemoteImage.Models.Screenshot Deserialize(ISerializationContext context, IValueReader reader)
        {
            var screenshot = new Drastic.MauiRemoteImage.Models.Screenshot();
            screenshot.Name = reader.ReadString();
            screenshot.Image = reader.ReadBytes();
            return screenshot;
        }

        public void Serialize(ISerializationContext context, IValueWriter writer, Drastic.MauiRemoteImage.Models.Screenshot element)
        {
            writer.WriteString(element.Name);
            writer.WriteBytes(element.Image);
        }
    }
}