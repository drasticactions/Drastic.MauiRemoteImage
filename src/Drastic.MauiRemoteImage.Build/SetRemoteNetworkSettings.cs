using System.Net.NetworkInformation;
using System.Net.Sockets;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Drastic.MauiRemoteImage.Build;

public class SetRemoteNetworkSettings : Microsoft.Build.Utilities.Task
{
    public string DllPath { get; set; }

    public override bool Execute()
    {
        try
        {
            var dllPath = this.DllPath ?? string.Empty;

            if (!File.Exists(dllPath))
            {
                // Stop right away.
                return true;
            }

            var ogDllPath = Path.Combine(Path.GetDirectoryName(dllPath)!, Path.GetFileNameWithoutExtension(dllPath) + "-original.dll");

            File.Replace(dllPath, ogDllPath, null);

            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(ogDllPath);
            TypeDefinition type = assembly.MainModule.Types.Single(t => t.FullName == "Drastic.MauiRemoteImage.Client.RemoteClientSettings");
            MethodDefinition method = type.Methods.Single(m => m.Name == "GetServerAddresses");
            Instruction emptyStringAssignment = method.Body.Instructions
                .Single(i => i.OpCode == OpCodes.Ldstr && (string)i.Operand == "");
            emptyStringAssignment.Operand = string.Join(";", DeviceIps);
            assembly.Write(this.DllPath);
            assembly.Dispose();
            File.Delete(ogDllPath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            System.Diagnostics.Debug.WriteLine(e);
        }

        return true;
    }

    static IEnumerable<string> DeviceIps => 
        GoodInterfaces()
        .SelectMany(x =>
            x.GetIPProperties().UnicastAddresses
                .Where(y => y.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(y => y.Address.ToString())).Union(new[] { "127.0.0.1" }).OrderBy(x => x);

    static IEnumerable<NetworkInterface> GoodInterfaces()
    {
        var allInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        return allInterfaces.Where(x => x.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                                        !x.Name.StartsWith("pdp_ip", StringComparison.Ordinal) &&
                                        x.OperationalStatus == OperationalStatus.Up);
    }
}