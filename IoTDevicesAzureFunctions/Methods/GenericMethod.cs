using System.Net;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IoTDevicesAzureFunctions.Methods
{
    public class GenericMethod
    {
        public readonly IConfiguration? _configuration;
        public readonly RegistryManager _registryManager;
        public readonly string IotHubConnectionString;
        public GenericMethod()
        {
            IotHubConnectionString = "HostName=kyh-iothub-gm.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=29hqWHz6b0gRxP/Oyo5q7rTahDG6r6sssAIoTDmJCmg=";
            _registryManager = RegistryManager
                .CreateFromConnectionString(IotHubConnectionString);
        }
    }
}
