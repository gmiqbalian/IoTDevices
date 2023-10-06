using IoTDevicesLibrary.Models;
using IoTDevicesLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Speakers
{
    public partial class MainWindow : Window
    {
        private readonly INetworkService _networkService;
        private readonly IDeviceService _deviceService;
        private readonly DeviceConfiguration _deviceConfiguration;
        public MainWindow(INetworkService networkService, IDeviceService deviceService, DeviceConfiguration deviceConfiguration)
        {
            InitializeComponent();

            _networkService = networkService;
            _deviceService = deviceService;
            _deviceConfiguration = deviceConfiguration;

            Task.WhenAll(TestConnectivityAsync(),
                ToggleSpeakersState());
        }

        private async Task ToggleSpeakersState()
        {
            Storyboard machine = (Storyboard)this.FindResource("MachineStoryboard");

            while (true)
            {
                if (_deviceConfiguration.IsConnectionAllowed)
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
}
