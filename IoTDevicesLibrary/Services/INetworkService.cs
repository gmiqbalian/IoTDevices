using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDevicesLibrary.Services
{
    public interface INetworkService
    {
        Task<string> TestConnectivityAsync(string ipAddress = "8.8.8.8");
    }
}
