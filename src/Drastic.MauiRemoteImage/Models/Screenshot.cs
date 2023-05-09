using Drastic.Tempest;

namespace Drastic.MauiRemoteImage.Models;

public class Screenshot: ISerializable
{
    public byte[] Image { get; set; }
    
    public string Name { get; set; }
    
    public void Deserialize(ISerializationContext context, IValueReader reader)
    {
        this.Name = reader.ReadString();
        this.Image = reader.ReadBytes();
    }

    public void Serialize(ISerializationContext context, IValueWriter writer)
    {
        writer.WriteString(this.Name);
        writer.WriteBytes(this.Image);
    }
}