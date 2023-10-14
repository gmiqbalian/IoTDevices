using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDevicesDataLibrary.Entities
{
    public class DeviceConfig
    {
        [Key]
        public string DeviceId { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
        public string? InstalledIn { get; set; }
        public string? Type { get; set; }
    }
}
