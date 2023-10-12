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
        private readonly IDeviceService _deviceService;
        private readonly DeviceConfiguration _deviceConfig;
        private readonly INetworkService _networkService;
        public MainWindow(IDeviceService deviceService, DeviceConfiguration deviceConfig, INetworkService networkService)
        {
            InitializeComponent();

            _deviceService = deviceService;
            _deviceConfig = deviceConfig;
            _networkService = networkService;

            Task.WhenAll(TestConnectivityAsync(),
                ToggleMusicIconStateAsync());
        }
        private async Task ToggleMusicIconStateAsync()
        {
            Storyboard tv = (Storyboard)this.FindResource("TvStoryboard");

            while(true)
            {
                if (_deviceConfig.IsSendingAllowed)
                {
                    MusicIcon.Visibility = Visibility.Visible;
                    tv.Begin();
                }
                else
                {
                    MusicIcon.Visibility = Visibility.Hidden;
                    tv.Stop();
                }
            
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
