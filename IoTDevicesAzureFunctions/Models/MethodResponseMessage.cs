using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System.Net;

namespace IoTDevicesAzureFunctions.Models;

public static class MethodResponseMessage
{
    public static HttpResponseData CreateReponseMessage(HttpRequestData req, HttpStatusCode statusCode, object content)
    {
        var response = req.CreateResponse(statusCode);

        if (statusCode == HttpStatusCode.OK)
        {
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(JsonConvert.SerializeObject(content));
            return response;
        }
        else
        {
            response = req.CreateResponse(HttpStatusCode.BadRequest);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(JsonConvert.SerializeObject(content));
            return response;
        }
    }
}
