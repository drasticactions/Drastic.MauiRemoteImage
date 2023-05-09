// <copyright file="DiagnosticMessage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Tempest;

namespace Drastic.MauiRemoteImage.Messages;

public abstract class DiagnosticMessage
    : Message
{
    protected DiagnosticMessage(DiagnosticsMessageType type)
        : base(DiagnosticsProtocol.Instance, (ushort)type)
    {
        this.Id = "Unknown";
    }

    public string Id { get; internal set; }

    /// <inheritdoc/>
    public override void ReadPayload(ISerializationContext context, IValueReader reader)
    {
        this.Id = reader.ReadString();
    }

    /// <inheritdoc/>
    public override void WritePayload(ISerializationContext context, IValueWriter writer)
    {
        writer.WriteString(this.Id);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{this.Id}: {this.GetType().ToString()}";
    }
}