using IoTDevicesLibrary.Models;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
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
        _deviceClient = DeviceClient.CreateFromConnectionString(_deviceConfig.ConnectionString);

        Task.FromResult(_deviceClient.SetMethodDefaultHandlerAsync(MethodResponseCallback, null));
    }

    private async Task<MethodResponse> MethodResponseCallback(MethodRequest req, object userContext)
    {
        var message = new ResponseMessage { Message = $"Method: {req.Name.ToUpper()} is executed." };

        switch (req.Name.ToLower())
        {
            case "start":
                _deviceConfig.IsConnectionAllowed = true;
                break;

            case "stop":
                _deviceConfig.IsConnectionAllowed = false;
                break;

            default:
                message.Message = $"Method: {req.Name.ToUpper()} is not found.";
                return new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)), 404);
        }

        return new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)), 200);
    }
}
