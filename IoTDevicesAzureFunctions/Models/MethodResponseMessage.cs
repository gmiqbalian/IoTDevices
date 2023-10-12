using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System.Net;

namespace IoTDevicesAzureFunctions.Models;

public static class MethodResponseMessage
{
    public static HttpResponseData CreateReponseMessage(HttpRequestData req, HttpStatusCode statusCode, string msg)
    {
        var response = req.CreateResponse(statusCode);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString(msg);

        return response;
    }
}
