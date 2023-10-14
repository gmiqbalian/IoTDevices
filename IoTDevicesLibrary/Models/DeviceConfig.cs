using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IoTDevicesLibrary.Models
{
    public class DeviceConfig
    {
        private string _connectionString;
        public string ConnectionString => _connectionString;
        public int TelemetryInterval { get; set; } = 10000; //change this to take as an argument in constructor
        public bool IsSendingAllowed { get; set; } = true;
        public bool IsDeviceConfigured { get; private set; }
        public event Action IsConfiguredChanged;
        public bool DeviceId { get; set; }
        public DeviceConfig(string url, string deviceId)
        {
            Task.FromResult(ConfigureDevice(url, deviceId));
        }
        public async Task ConfigureDevice(string url, string deviceId)
        {
            try
            {
                var data = JsonConvert.SerializeObject(new { deviceId = deviceId });
                HttpContent content = new StringContent(data);

                using var _httpClient = new HttpClient();
                var result = await _httpClient.PostAsync(url, content);
                if (result.IsSuccessStatusCode)
                {
                    _connectionString = await result.Content.ReadAsStringAsync();
                    IsDeviceConfigured = true;

                    IsConfiguredChanged.Invoke();
                }
                else
                    IsDeviceConfigured = false;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
    }
}
