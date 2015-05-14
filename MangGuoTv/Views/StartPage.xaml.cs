using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;

namespace MangGuoTv.Views
{
    public partial class StartPage : PhoneApplicationPage
    {
        private DispatcherTimer BgImgTimer = new DispatcherTimer();
        private int time = 2;
        public StartPage()
        {
            InitializeComponent();
            versionText.Text = DeviceUtil.GetAppVersion();
            BgImgTimer.Interval = new TimeSpan(1000);
            BgImgTimer.Tick += new EventHandler(BgImgTimer_Tick);
            BgImgTimer.Start();
        }

        private void BgImgTimer_Tick(object sender, EventArgs e)
        {
            if (time != 0) 
            {
                time--;
                return;
            }
            BgImgTimer.Stop();
            this.Dispatcher.BeginInvoke(() => 
            {
                this.NavigationService.Navigate(new Uri(CommonData.MianPage, UriKind.RelativeOrAbsolute));
            });
        }
    }
}