using IoTDevicesLibrary.Services;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Television
{
    public partial class MainWindow : Window
    {
        private readonly DeviceService _deviceService;
        private readonly NetworkService _networkService;
        public MainWindow(DeviceService deviceService, NetworkService networkService)
        {
            InitializeComponent();

            _deviceService = deviceService;
            _networkService = networkService;

            Task.WhenAll(ConfigureDevice(),
                GetConnectionStatusAsync(),
                ToggleMusicIconStateAsync(),
                SendTelemetryDataAsync());
        }
        private async Task ConfigureDevice()
        {
            var twinCollection = new TwinCollection();
            twinCollection["deviceType"] = "TV";
            twinCollection["location"] = "lounge";

            if (_deviceService.IsConfigured)
            {
                await _deviceService.UpdateTwinAsync(twinCollection);
                await _deviceService.RegisterDirectMethodToCloud();
            }
        }
        private async Task ToggleMusicIconStateAsync()
        {
            Storyboard tv = (Storyboard)this.FindResource("TvStoryboard");

            while(true)
            {
                if (_deviceService.IsSendingAllowed)
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
        private async Task GetConnectionStatusAsync()
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
        private async Task SendTelemetryDataAsync()
        {
            while (true)
            {
                if (_deviceService.IsSendingAllowed)
                {
                    var payload = new
                    {
                        DeviceState = "active",
                        TimeStamp = DateTime.Now
                    };

                    await _deviceService.SendDataAsync(JsonConvert.SerializeObject(payload));

                    
                    await Task.Delay(1000);
                }
                else
                    await _deviceService.SendDataAsync(JsonConvert.SerializeObject("Sending not allowed."));
            }
        }
    }
}
