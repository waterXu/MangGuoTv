using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace MangGuoTv.Popups
{
    public partial class CommentControl : UserControl
    {
        public CommentControl()
        {
            InitializeComponent();
        }

        private void Fankui_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.OffPopUp();
            string strForamt = string.Format(" Device Info：\r\n {0} \r\n OS Version: \r\n {1} \r\n App Version:{2} \r\n NetWorkInfo:\r\n {3} \r\n "
                           , DeviceUtil.GetDeviceName() + DeviceUtil.GetManufactor(), DeviceUtil.GetOSVersion(), DeviceUtil.GetAppVersion(), DeviceUtil.GetNetWorkType());

            EmailComposeTask emailTask = new EmailComposeTask()
            {
                To = "xuyanlan@outlook.com",
                Subject = "芒果TV意见反馈",
                Body = strForamt,
            };
            try
            {
                emailTask.Show();
            }
            catch 
            {

            }
        }

        private void Grade_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.OffPopUp();
            try
            {
                Windows.System.Launcher.LaunchUriAsync(new Uri("zune:reviewapp?appid=30c60bf9-e4c2-412e-8cc0-b53d15618f63"));

                //Windows.System.Launcher.LaunchUriAsync(new Uri("zune:reviewapp?appid=c700c407-1d91-4007-8474-b24271e25661"));
            }
            catch
            {

            }
        }

 
    }
}
