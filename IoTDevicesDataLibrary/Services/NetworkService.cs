using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IoTDevicesDataLibrary.Services
{
    public class NetworkService
    {
        public async Task<string> TestConnectivityAsync(string ipAddress = "8.8.8.8")
        {
            bool connectionStatus;

            try
            {
                using var ping = new Ping();
                var response = await ping.SendPingAsync(ipAddress, 1000, new byte[32], new());

                connectionStatus = response.Status == IPStatus.Success;
            }
            catch { connectionStatus = false; }

            return connectionStatus ? "Connected" : "Disconnected";
        }
    }
}
