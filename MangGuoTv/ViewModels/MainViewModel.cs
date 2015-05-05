using MangGuoTv.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv.ViewModels
{
    public class MainViewModel
    {
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
    }
}
