using MangGuoTv.Commands;
using MangGuoTv.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MangGuoTv.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            this.AllChannels = new List<ChannelInfo>();
            //BuildLocalizedApplicationBar();
           
        }
        public void LoadChannels() 
        {
            string strFileContent = string.Empty;
            if (!CommonData.ChannelLoaded)
            {
                if (WpStorage.isoFile.FileExists(CommonData.ChannelStorage))
                {
                    strFileContent = WpStorage.ReadIsolatedStorageFile(CommonData.ChannelStorage);
                }
                else
                {
                    using (Stream stream = Application.GetResourceStream(new Uri("channels.txt", UriKind.Relative)).Stream)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            strFileContent = reader.ReadToEnd();
                        }
                    }
                }

                AllChannelsData channels = JsonConvert.DeserializeObject<AllChannelsData>(strFileContent);
                CommonData.LockedChannel = channels.lockedChannel;
                CommonData.NormalChannel = channels.normalChannel;
            }
            this.AllChannels.AddRange(CommonData.LockedChannel);
            this.AllChannels.AddRange(CommonData.NormalChannel);
        }
        public List<ChannelInfo> AllChannels { get; set; }

        #region Command

        private DelegateCommand<string> _SelectVideoCommand;
        public DelegateCommand<string> SelectVideoCommand
        {
            get
            {
                return _SelectVideoCommand ?? (_SelectVideoCommand = new DelegateCommand<string>((playerUrl) =>
                {
                    System.Diagnostics.Debug.WriteLine(playerUrl);
                }));
            }
        }
        #endregion
    }
}
