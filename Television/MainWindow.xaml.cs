using IoTDevicesLibrary.Models;
using IoTDevicesLibrary.Services;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Television
{
    public partial class MainWindow : Window
    {
        private readonly DeviceService _deviceManager;
        private readonly DeviceConfiguration _deviceConfig;
        private readonly NetworkService _networkService;
        public MainWindow(DeviceService deviceManager, DeviceConfiguration deviceConfig, NetworkService networkService)
        {
            InitializeComponent();

            _deviceManager = deviceManager;
            _deviceConfig = deviceConfig;
            _networkService = networkService;

            Task.WhenAll(TestConnectivityAsync(),
                ToggleMusicIconStateAsync());
        }
        private async Task ToggleMusicIconStateAsync()
        {
            while(true)
            {
                if(_deviceConfig.IsConnectionAllowed)
                    MusicIcon.Visibility = Visibility.Visible;
                else
                    MusicIcon.Visibility = Visibility.Hidden;
            
                await Task.Delay(5000);
            }
        }
        private async Task TestConnectivityAsync()
        {
            while (true)
            {
                var status = await _networkService.TestConnectivityAsync();
                ConnectivityStatus.Text = status;

                if(status.ToLower() == "connected")
                    ConnectivityStatus.Background = Brushes.Green;
                else if(status.ToLower() == "disconnected")
                    ConnectivityStatus.Background = Brushes.Red;

                await Task.Delay(1000);
            }
        }
    }
}
