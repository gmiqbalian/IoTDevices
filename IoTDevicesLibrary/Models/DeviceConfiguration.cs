namespace IoTDevicesLibrary.Models;

public class DeviceConfiguration
{
    private readonly string _connectionString;
    public string ConnectionString => _connectionString;
    public int TelemetryInterval { get; set; } = 10000; //change this to take as an argument in constructor
    public bool IsSendingAllowed { get; set; } = true;
    public bool DeviceId { get; set; }
    public DeviceConfiguration(string connectionString)
    {
        _connectionString = connectionString;
    }

}
