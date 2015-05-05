using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace MangGuoTv.PopUp
{
    public partial class DbRegisterControl : UserControl
    {
        public DbRegisterControl()
        {
            InitializeComponent();
        }

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            //TextBox input = sender as TextBox;
            //if (input == null)
            //{
            //    return;
            //}
            //if (input.Text == AppResources.RegisterTip || input.Text == AppResources.PasswordTip)
            //{
            //    input.Text = "";
            //}
            PopupManager.Input_GotFocus((Control)sender, this.LayoutRoot);
        }

        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            PopupManager.Input_LostFocus(this.LayoutRoot);
        }

        private void HaveAccount_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.ShowUserControl(PopupManager.UserControlType.LoginControl);
        }
        string userName;
        string password;
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            //user_name=xuyanlan1&password=xuyanlan1&confirm_password=xuyanlan1&captcha_solution=country&captcha_id=o5ehRbcmiHsF9i2QeM9kmxgt%3Aen&agreement=on
            //userName = Account.Text.Trim();
            //if (string.IsNullOrEmpty(userName) || userName==AppResources.RegisterTip)
            //{
            //    MessageBox.Show(AppResources.AccountEmpty);
            //    return;
            //}
            //password = AccountPwdInput.Text.Trim();
            //if (string.IsNullOrEmpty(password) || password == AppResources.PasswordTip)
            //{
            //    MessageBox.Show(AppResources.PasswordEmpty);
            //    return;
            //}
            //string repassword = ReAccountPwdInput.Text.Trim();
            //if (string.IsNullOrEmpty(repassword))
            //{
            //    MessageBox.Show(AppResources.PasswordEmpty);
            //    return;
            //}
            //if (!password.Equals(repassword))
            //{
            //    MessageBox.Show(AppResources.PasswordEqual);
            //    return;
            //}
            //string captcha = AuthCodeInput.Text.Trim();
            //if (string.IsNullOrEmpty(captcha))
            //{
            //    MessageBox.Show(AppResources.CaptchaTip);
            //    return;
            //}
            //if (!(bool)LicenseCheckBox.IsChecked)
            //{
            //    MessageBox.Show(AppResources.AgreementTip);
            //    return;
            //}
            //string registerUrl = DbFMCommonData.RegisterUrl;
            // registerUrl +="&user_name=" + userName;
            //registerUrl += "&password=" + password + "&confirm_password=" + repassword;
            //registerUrl += "&captcha_solution=" + captcha;
            //registerUrl += "&aptcha_id=" + DbFMCommonData.CaptchaId;
            //registerUrl += "&agreement=on";
            //System.Diagnostics.Debug.WriteLine("注册请求url：" + registerUrl);
            //HttpHelper.httpPost(registerUrl, new AsyncCallback(RegisterResult));
        }
        private void RegisterResult(IAsyncResult ar)
        {
            //this.Dispatcher.BeginInvoke(() =>
            //{
            //    try
            //    {
            //        if (HttpHelper.LoginResultCodeInfo(ar))
            //        {
            //            WpStorage.SetIsoSetting(DbFMCommonData.UserName, userName);
            //            WpStorage.SetIsoSetting(DbFMCommonData.Password, password);
            //           // DbFMCommonData.loginSuccess = true;
            //            DbFMCommonData.informCallback((int)DbFMCommonData.CallbackType.Login, true);
            //        }
            //        else
            //        {
            //            App.ShowToast(AppResources.OperationError);
            //            return;
            //        }
            //    }
            //    catch
            //    {
            //        MessageBox.Show(AppResources.OperationError);
            //        DbFMCommonData.informCallback((int)DbFMCommonData.CallbackType.Login, false);
            //    }
            //});
        }

        private void ShowLicense_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string url = "http://www.douban.com/about/agreement";
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri(url, UriKind.Absolute);
            try
            {
                task.Show();
            }
            catch (Exception ex)
            {

            }
        }

        private void DeleteImg_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PopupManager.OffPopUp();
        }

        private void GetAuthCode_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //try
            //{
            //    System.Diagnostics.Debug.WriteLine("注册页面GetcaptchaId请求url：" + DbFMCommonData.GetcaptchaId);
            //    HttpHelper.httpGet(DbFMCommonData.GetcaptchaId, new AsyncCallback((ar) =>
            //    {
            //        string result = HttpHelper.SyncResultTostring(ar);
            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            result = result.Replace("\"", "");
            //            DbFMCommonData.CaptchaId = result;
            //            DbFMCommonData.CaptchaImgUrl = DbFMCommonData.GetcaptchaImgUrl + result;
            //            this.Dispatcher.BeginInvoke(() =>
            //            {
            //                GetAuthCode.Source = new BitmapImage(new Uri(DbFMCommonData.CaptchaImgUrl, UriKind.Absolute));
            //            });
            //        }
            //        else
            //        {
            //            App.ShowToast(AppResources.OperationError);
            //        }

            //    }), "wp");
            //}
            //catch
            //{
            //    App.ShowToast(AppResources.OperationError);
            //}
        }

        private void GetAuthCode_Loaded(object sender, RoutedEventArgs e)
        {
            //if (DbFMCommonData.CaptchaImgUrl != null)
            //{
            //    try
            //    {
            //        if (DbFMCommonData.CaptchaImgUrl == null)
            //        {
            //            return;
            //        }
            //        Dispatcher.BeginInvoke(() =>
            //        {
            //            GetAuthCode.Source = new BitmapImage(new Uri(DbFMCommonData.CaptchaImgUrl, UriKind.Absolute));
            //        });
            //    }
            //    catch
            //    {
            //        App.ShowToast(AppResources.OperationError);
            //    }
            //}

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    System.Diagnostics.Debug.WriteLine("注册页面GetcaptchaId请求url：" + DbFMCommonData.GetcaptchaId);
            //    HttpHelper.httpGet(DbFMCommonData.GetcaptchaId, new AsyncCallback((ar) =>
            //    {
            //        string result = HttpHelper.SyncResultTostring(ar);
            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            result = result.Replace("\"", "");
            //            DbFMCommonData.CaptchaId = result;
            //            DbFMCommonData.CaptchaImgUrl = DbFMCommonData.GetcaptchaImgUrl + result;
            //            this.Dispatcher.BeginInvoke(() =>
            //            {
            //                GetAuthCode.Source = new BitmapImage(new Uri(DbFMCommonData.CaptchaImgUrl, UriKind.Absolute));
            //            });
            //        }
            //        else
            //        {
            //            App.ShowToast(AppResources.OperationError);
            //        }

            //    }), "wp");
            //}
            //catch
            //{
            //    App.ShowToast(AppResources.OperationError);
            //}
        }
    }
}
