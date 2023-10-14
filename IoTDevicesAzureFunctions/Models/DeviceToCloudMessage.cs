using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace IoTDevicesAzureFunctions.Models
{
    public class DeviceToCloudMessage
    {
        public string MessageId { get; set; }
        public string? DeviceId { get; set; }
        public string? Payload { get; set; }
        public string PartitionKey { get; set; } = "Message";
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
