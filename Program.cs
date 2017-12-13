using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;

namespace Trigger_Reboot
{
    class Program
    {
        static RegistryManager registryManager;
        static string connString = "HostName=PlantMonitoringIoTHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=nMdz3etWKEZeJ9d9prpKrJneyP0x/aK6ytadZMUDc+k=";
        static ServiceClient client;
        static string targetDevice = "DeviceManagementDevID";

        //Gets the device twin for rebooting the device and outputs the reported properties
        public static async Task QueryTwinRebootReported()
        {
            Twin twin = await registryManager.GetTwinAsync(targetDevice);
            Console.WriteLine(twin.Properties.Reported.ToJson());
        }

        //Initiates the reboot on the device using a direct method
        public static async Task StartReboot()
        {
            client = ServiceClient.CreateFromConnectionString(connString);
            CloudToDeviceMethod method = new CloudToDeviceMethod("reboot");
            method.ResponseTimeout = TimeSpan.FromSeconds(30);

            CloudToDeviceMethodResult result = await client.InvokeDeviceMethodAsync(targetDevice, method);

            Console.WriteLine("Invoked firmware update on device.");
        }

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connString);
            StartReboot().Wait();
            QueryTwinRebootReported().Wait();
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
