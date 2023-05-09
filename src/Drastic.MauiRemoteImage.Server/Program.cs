
using System.Net.Sockets;
using CommunityToolkit.Mvvm.DependencyInjection;
using Drastic.MauiRemoteImage.Server.Services;
using Drastic.MauiRemoteImage.Server.ViewModels;
using Drastic.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sharprompt;

Console.WriteLine("Drastic.MauiRemoteImage.Server");

Ioc.Default.ConfigureServices(
    new ServiceCollection()
        .AddLogging((factory) =>
        {
            factory.AddConsole();
        })
        .AddSingleton<IErrorHandlerService>(new ServerErrorHandler())
        .AddSingleton<IAppDispatcher>(new AppDispatcher())
        .AddSingleton<MainViewModel>()
        .BuildServiceProvider());

var vm = Ioc.Default.GetService<MainViewModel>();

await vm.OnLoad();

var selectedInterface = Prompt.Select("Select Network Interface", vm.NetworkInterfaces, textSelector: (netInterface)
    => $"{netInterface.Name} - {netInterface.GetIPProperties().UnicastAddresses.Where(y => y.Address.AddressFamily == AddressFamily.InterNetwork).Select(y => y.Address.ToString()).FirstOrDefault() ?? "(Empty)"}");

Console.WriteLine(selectedInterface.Name);

var port = Prompt.Input<int>("Enter Port Number", 8888);

Console.WriteLine(port);

vm.SelectedInterface = selectedInterface;
vm.Port = port;

await vm.StartServerCommand.ExecuteAsync(vm.SelectedInterface);

Console.WriteLine("Press enter to request an image, press any other key to exit.");

while (true)
{
    var keyInfo = Console.ReadKey(true);

    if (keyInfo.Key != ConsoleKey.Enter)
    {
        break;
    }
    
    Console.WriteLine("Sending Screenshot Request...");
    vm.SendScreenshotRequest();
}

