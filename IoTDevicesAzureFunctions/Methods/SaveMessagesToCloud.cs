using System;
using System.Text;
using Azure.Messaging.EventHubs;
using IoTDevicesAzureFunctions.DataContext;
using IoTDevicesAzureFunctions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace IoTDevicesAzureFunctions.Methods
{
    public class SaveMessagesToCloud
    {
        private readonly CosmosDbContext _dbContext;
        public SaveMessagesToCloud(CosmosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Function(nameof(SaveMessagesToCloud))]
        public async Task Run([EventHubTrigger("iothub-ehub-kyh-iothub-25232418-35685cca60", Connection = "IotHubEndpoint")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                var message = new DeviceToCloudMessage
                {
                    Payload = Encoding.UTF8.GetString(@event.Body.ToArray()),
                    CreatedOn = DateTime.Now,
                    DeviceId = @event.SystemProperties["iothub-connection-device-id"].ToString()
                };
                _dbContext.Add(message);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
