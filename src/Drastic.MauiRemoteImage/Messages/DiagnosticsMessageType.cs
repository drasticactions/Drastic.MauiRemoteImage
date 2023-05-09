namespace Drastic.MauiRemoteImage.Messages;

public enum DiagnosticsMessageType
    : ushort
{
    TestRequest = 1,
    TestResponse = 2,
    ClientRegistration = 3,
    OnScreenshotRequest = 4,
    OnScreenshotResponse = 5,
    AppClientDiscoveryResponse = 6,
    AppClientDisconnect = 7,
    AppClientConnect = 8,
}
