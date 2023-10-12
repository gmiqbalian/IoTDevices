using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDevicesLibrary.Models
{
    public class DeviceToCloudMessage
    {
        public string? DeviceId { get; set; }
        public string? Payload { get; set; }
        public string PartitionKey { get; set; } = "Messages";
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
