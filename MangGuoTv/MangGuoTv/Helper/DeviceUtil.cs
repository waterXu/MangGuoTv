using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;

namespace MangGuoTv
{
    public class DeviceUtil
    {
        /// <summary>
        /// 获取程序集信息
        /// </summary>
        /// <returns>返回程序版本号信息</returns>
        public static string GetAppVersion()
        {
            var asm = Assembly.GetExecutingAssembly();
            var parts = asm.FullName.Split(',');
            return parts[1].Split('=')[1];
        }

        public static string GetDeviceName()
        {
            string deviceName = DeviceExtendedProperties.GetValue("DeviceName").ToString();
            deviceName = deviceName.Replace(" ", "-");
            return deviceName;
        }

        public static string GetManufactor()
        {
            string manufacturer = DeviceExtendedProperties.GetValue("DeviceManufacturer").ToString();
            manufacturer = manufacturer.Replace(" ", "-");
            return manufacturer;
        }

        public static string GetOSVersion()
        {
            string osVersion = System.Environment.OSVersion.ToString().Replace(" ", "-");
            return osVersion;
        }

        public static string GetNetWorkType()
        {
            var net = NetworkInterface.NetworkInterfaceType;
            string networkInfo = net.ToString();
            return networkInfo;
        }
    }
}
