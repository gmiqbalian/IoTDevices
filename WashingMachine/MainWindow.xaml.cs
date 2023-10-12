using IoTDevicesLibrary.Models;
using IoTDevicesLibrary.Services;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WashingMachine;

public partial class MainWindow : Window
{
    private readonly NetworkService _networkService;
    private readonly DeviceService _deviceService;
    private readonly DeviceConfiguration _deviceConfiguration;
    public MainWindow(NetworkService networkService, DeviceService deviceService, DeviceConfiguration deviceConfiguration)
    {
        InitializeComponent();

        _networkService = networkService;
        _deviceService = deviceService;
        _deviceConfiguration = deviceConfiguration;

        Task.WhenAll(TestConnectivityAsync(),
            ToggleMachineState());
    }
    private async Task ToggleMachineState()
    {
        Storyboard machine = (Storyboard)this.FindResource("MachineStoryboard");
        
        while (true)
        {
            if (_deviceConfiguration.IsSendingAllowed)
                machine.Begin();
            else
                machine.Stop();

            await Task.Delay(5000);
        }
    }
    private async Task TestConnectivityAsync()
    {
        while (true)
        {
            var status = await _networkService.TestConnectivityAsync();
            ConnectivityStatus.Text = status;

            if (status.ToLower() == "connected")
                ConnectivityStatus.Background = Brushes.Green;
            else if (status.ToLower() == "disconnected")
                ConnectivityStatus.Background = Brushes.Red;

            await Task.Delay(1000);
        }
    }
}
