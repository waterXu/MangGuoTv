using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MangGuoTv.Views
{
    public partial class DownVideo : PhoneApplicationPage
    {
        public DownVideo()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CallbackManager.currentPage = this;
            this.DataContext = App.DownVideoModel;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            CallbackManager.currentPage = null;
            this.DataContext = null;
        }
        private void DownedVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}