using IoTDevicesLibrary.Models;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace IoTDevicesLibrary.Services;

public class DeviceService : IDeviceService
{
    private readonly DeviceConfiguration _deviceConfig;
    private readonly DeviceClient _deviceClient;
    public DeviceService(DeviceConfiguration deviceConfig)
    {
        _deviceConfig = deviceConfig;
        _deviceClient = DeviceClient.CreateFromConnectionString(_deviceConfig.ConnectionString, TransportType.Mqtt);

        Task.FromResult(_deviceClient.SetMethodDefaultHandlerAsync(MethodResponseCallback, null));
    }

    private async Task<MethodResponse> MethodResponseCallback(MethodRequest req, object userContext)
    {
        var reponseToCloud = new ResponseMessage { Message = $"Method: {req.Name.ToUpper()} is executed." };

        try
        {
            switch (req.Name.ToLower())
            {
                case "start":
                    _deviceConfig.IsSendingAllowed = true;
                    break;

                case "stop":
                    _deviceConfig.IsSendingAllowed = false;
                    break;

                default:
                    reponseToCloud.Message = $"Method: {req.Name.ToUpper()} is not found.";
                    return new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reponseToCloud)), 404);
            }

            return new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reponseToCloud)), 200);
        }
        catch (Exception e) 
        { 
            reponseToCloud.Message = $"Error occured: {e.Message}";
            return new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reponseToCloud)), 400);
        }
    }
    public async Task<bool> SendDataAsync(string payload)
    {
        try
        {
            var messageToCloud = new Message(Encoding.UTF8.GetBytes(payload));
            await _deviceClient.SendEventAsync(messageToCloud);
            
            await Task.Delay(1000);
            return true;
        }
        catch (Exception e) { Debug.WriteLine(e.Message); }
        return false;
    }
}
