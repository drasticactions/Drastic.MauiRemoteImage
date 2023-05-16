namespace Drastic.MauiRemoteImage.Client;

public static class RemoteClientSettings
{
    public static string[] GetServerAddresses()
    {
        var addresses = "";

        if (string.IsNullOrEmpty(addresses))
        {
            return new string[0];
        }

        return addresses.Split(";");
    }
}