# Drastic.MauiRemoteImage

Drastic.MauiRemoteImage is a server/client application that lets you remotely take screenshots of MAUI apps.

## Setup Server

To setup the server, build the `Drastic.MauiRemoteImage.Server` application. Once built, run the app and follow the prompts to set your Network Interface and Port.

Once you set the interface and port, copy the IP address.

## Setup Client

[![NuGet Version](https://img.shields.io/nuget/v/Drastic.MauiRemoteImage.Client.svg)](https://www.nuget.org/packages/Drastic.Drastic.MauiRemoteImage.Client/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

Install the Drastic.MauiRemoteImage.Client nuget, or reference the Drastic.MauiRemoteImage.Client source code directly in your app.

Once installed, you need to initialize the client. You can do this by creating a new `AppClient`.


```csharp
		this.client = new AppClient("TestClient");
        // 8888 = port number
        // Async Command, can be fired off on any thread.
		this.client.ConnectAsync(new Drastic.Tempest.Target("IP ADDRESS", 8888));
```

If the server is running, the client should connect.

## Full Cycle

Once the server and client are connected, return to the server app. Press `Enter` to call for a new image. All connected clients should respond by sending screenshots of all of their open windows. Once done, press any other key to close the server.