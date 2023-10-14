using System.Diagnostics;
using System.Net;
using IoTDevicesAzureFunctions.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IoTDevicesAzureFunctions.Methods
{
    public class GetDeviceFromCloud : GenericMethod
    {
        public GetDeviceFromCloud()
        {
        }

        [Function("GetDeviceFromCloud")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetDevice")] HttpRequestData req)
        {
            try
            {
                var response = req.CreateResponse(HttpStatusCode.OK);

                var deviceIdFromUrl = req.Query["deviceId"];
                if (!string.IsNullOrEmpty(deviceIdFromUrl))
                {
                    var device = await _registryManager.GetDeviceAsync(deviceIdFromUrl);
                
                    if(device is not null)
                        return MethodResponseMessage.CreateReponseMessage(req, HttpStatusCode.OK, JsonConvert.SerializeObject(device));
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return MethodResponseMessage.CreateReponseMessage(req, HttpStatusCode.BadRequest, "Bad Request");
        }
    }
}
