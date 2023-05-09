using Drastic.Tempest;

namespace Drastic.MauiRemoteImage;

public static class DiagnosticsProtocol
{
    public static readonly Protocol Instance = new Protocol(42, 1);

    static DiagnosticsProtocol()
    {
        Instance.Discover();
    }
}