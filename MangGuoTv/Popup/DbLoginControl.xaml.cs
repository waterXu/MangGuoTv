using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using Microsoft.Phone.Shell;
using System.IO;
using MangGuoTv.Resources;
using Microsoft.Phone.Tasks;

namespace MangGuoTv.PopUp
{
    public partial class DbLoginControl : UserControl
    {
        public DbLoginControl()
        {
            InitializeComponent();
        }

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox input = sender as TextBox;
            if (input == null)
            {
                return;
            }
            //if (input.Text == AppResources.AccountTip)
            //{
            //    input.Text = "";
            //}
            PopupManager.Input_GotFocus((Control)sender,this.LayoutRoot);
        }
        private void PassWordInput_GotFocus(object sender, RoutedEventArgs e)
        {
            PopupManager.Input_GotFocus((Control)sender, this.LayoutRoot);
        }
        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            PopupManager.Input_LostFocus(this.LayoutRoot);
        }
        string username = null;
        string password = null;
        bool isLogining = false;
        private void DbFMLogin_Click(object sender, RoutedEventArgs e)
        {
            //if (isLogining)
            //{
            //    return;
            //}
            //else
            //{
            //    isLogining = true;
            //}
            //username = DbFmAccount.Text.Trim();
            //if (string.IsNullOrEmpty(username) || username == AppResources.AccountTip)
            //{
            //    MessageBox.Show(AppResources.AccountEmpty);
            //    return;
            //}
            //password = DbFmPassword.Password.Trim();
            //if (string.IsNullOrEmpty(password))
            //{
            //    MessageBox.Show(AppResources.PasswordEmpty);
            //    return;
            //}
            //string loginUrlInfo = DbFMCommonData.LoginUrl + "?app_name=" + DbFMCommonData.AppName + "&version=" + DbFMCommonData.Version;
            //loginUrlInfo += "&email=" + username + "&password=" + password;
            //System.Diagnostics.Debug.WriteLine("登录请求url：" + loginUrlInfo);
            //HttpHelper.httpGet(loginUrlInfo, new AsyncCallback(LoginResult));
        }
        private void LoginResult(IAsyncResult ar)
        {
            //this.Dispatcher.BeginInvoke(() => 
            //{
            //    try
            //    {
            //        if (HttpHelper.LoginResultCodeInfo(ar))
            //        {
            //            WpStorage.SetIsoSetting(DbFMCommonData.UserName, username);
            //            WpStorage.SetIsoSetting(DbFMCommonData.Password, password);
            //            DbFMCommonData.loginSuccess = true;
            //        }
            //        else
            //        {
            //            App.ShowToast(AppResources.OperationError);
            //        }
            //        DbFMCommonData.informCallback((int)DbFMCommonData.CallbackType.Login, DbFMCommonData.loginSuccess);
            //    }
            //    catch
            //    {
            //        App.ShowToast(AppResources.OperationError);
            //        DbFMCommonData.informCallback((int)DbFMCommonData.CallbackType.Login, false);
            //    }
            //    isLogining = false;
            //});
        }

        private void RegisterAccount_Click(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("注册页面GetcaptchaId请求url：" + DbFMCommonData.GetcaptchaId);
            //try
            //{
            //    HttpHelper.httpGet(DbFMCommonData.GetcaptchaId, new AsyncCallback((ar) =>
            //    {
            //        string result = HttpHelper.SyncResultTostring(ar);
            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            result = result.Replace("\"","");
            //            DbFMCommonData.CaptchaId = result;
            //            DbFMCommonData.CaptchaImgUrl = DbFMCommonData.GetcaptchaImgUrl + result;
            //        }
            //        else
            //        {
            //            //App.ShowToast(AppResources.OperationError);
            //        }
            //        this.Dispatcher.BeginInvoke(() =>
            //        {
            //            PopupManager.ShowUserControl(PopupManager.UserControlType.RegisterControl);
            //        });
            //    }));
            //}
            //catch
            //{
            //    PopupManager.ShowUserControl(PopupManager.UserControlType.RegisterControl);
            //}
        }

        private void ForgetPwd_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }
    }
}
