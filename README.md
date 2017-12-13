Back-end apps can use Azure IoT Hub primitives, such as device twin and direct methods, to remotely start and monitor device management actions on devices. This tutorial shows you how a back-end app and a device app can work together to initiate and monitor a remote device reboot using IoT Hub.

Use a direct method to initiate device management actions (such as reboot, factory reset, and firmware update) from a back-end app in the cloud. The device is responsible for:

Handling the method request sent from IoT Hub.
Initiating the corresponding device-specific action on the device.
Providing status updates through reported properties to IoT Hub.
You can use a back-end app in the cloud to run device twin queries to report on the progress of your device management actions.

This project deals with:

Use the Azure portal to create an IoT Hub and create a device identity in your IoT hub.
Create a simulated device app that contains a direct method that reboots that device. Direct methods are invoked from the cloud.
Create a .NET console app that calls the reboot direct method in the simulated device app through your IoT hub.
At the end of this tutorial, you have two .NET console apps:

SimulateManagedDevice, which connects to your IoT hub with the device identity created earlier, receives a reboot direct method, simulates a physical reboot, and reports the time for the last reboot.
TriggerReboot, which calls a direct method in the simulated device app, displays the response, and displays the updated reported properties.
To complete this tutorial, you need the following:

Visual Studio 2015 or Visual Studio 2017.
An active Azure account. (If you don't have an account, you can create a free account in just a couple of minutes.)
