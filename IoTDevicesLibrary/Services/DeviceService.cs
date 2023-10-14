using IoTDevicesLibrary.Models;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IoTDevicesLibrary.Services;

public class DeviceService
{
    private DeviceClient? _deviceClient;
    public bool IsConfigured { get; private set; }
    public event Action? IsConfiguredChanged;
    public bool IsSendingAllowed;

    public DeviceService(string url, string deviceId)
    {
        Task.FromResult(SetupDevice(url, deviceId));
    }
    public async Task SetupDevice(string url, string deviceId)
    {
        try
        {
            var data = JsonConvert.SerializeObject(new { deviceId = deviceId });
            HttpContent content = new StringContent(data);

            using var _httpClient = new HttpClient();
            var result = await _httpClient.PostAsync(url, content);
            if (result.IsSuccessStatusCode)
            {
                var connectionString = await result.Content.ReadAsStringAsync();
                _deviceClient = DeviceClient.CreateFromConnectionString(connectionString);
                IsConfigured = true;
            }
            else
                IsConfigured = false;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
    }
    public async Task UpdateTwinAsync(TwinCollection twinCollection)
    {
        if(IsConfigured)
            await _deviceClient!.UpdateReportedPropertiesAsync(twinCollection);
    }
    public async Task RegisterDirectMethodToCloud()
    {
        if (IsConfigured)
            await _deviceClient!.SetMethodDefaultHandlerAsync(MethodResponseCallback, null);
    }
    private async Task<MethodResponse> MethodResponseCallback(MethodRequest req, object userContext)
    {
        var reponseToCloud = new ResponseMessage { Message = $"Method: {req.Name.ToUpper()} is executed." };
        var twinCollection = new TwinCollection();

        try
        {
            switch (req.Name.ToLower())
            {
                case "start":
                    IsSendingAllowed = true;
                    twinCollection["state"] = "active";
                    await _deviceClient!.UpdateReportedPropertiesAsync(twinCollection);
                    break;

                case "stop":
                    IsSendingAllowed = false;
                    twinCollection["state"] = "inactive";
                    await _deviceClient!.UpdateReportedPropertiesAsync(twinCollection);
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
            await _deviceClient!.SendEventAsync(messageToCloud);

            await Task.Delay(1000);
            return true;
        }
        catch (Exception e) { Debug.WriteLine(e.Message); }
        return false;
    }
}
