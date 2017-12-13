using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;

namespace SimulatedManagedDevice
{
    class Program
    {
        static string DeviceConnectionString = "HostName=PlantMonitoringIoTHub.azure-devices.net;DeviceId=DeviceManagementDevID;SharedAccessKey=L8zI1C++4jeTzS0aQt3PxMKULKL7gep/hrf/dIQkQ+s=";
        static DeviceClient Client = null;

        static Task<MethodResponse> onReboot(MethodRequest methodRequest, object userContext)
        {
            // In a production device, you would trigger a reboot scheduled to start after this method returns
            // For this sample, we simulate the reboot by writing to the console and updating the reported properties 
            try
            {
                Console.WriteLine("Rebooting!");

                // Update device twin with reboot time. 
                TwinCollection reportedProperties, reboot, lastReboot;
                lastReboot = new TwinCollection();
                reboot = new TwinCollection();
                reportedProperties = new TwinCollection();
                lastReboot["lastReboot"] = DateTime.Now;
                reboot["reboot"] = lastReboot;
                reportedProperties["iothubDM"] = reboot;
                Client.UpdateReportedPropertiesAsync(reportedProperties).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);
            }

            string result = "'Reboot started.'";
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));
        }
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Connecting to hub");
                Client = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);

                // setup callback for "reboot" method
                Client.SetMethodHandlerAsync("reboot", onReboot, null).Wait();
                Console.WriteLine("Waiting for reboot method\n Press enter to exit.");
                Console.ReadLine();

                Console.WriteLine("Exiting...");

                // as a good practice, remove the "reboot" handler
                Client.SetMethodHandlerAsync("reboot", null, null).Wait();
                Client.CloseAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);
            }
        }
    }
}
