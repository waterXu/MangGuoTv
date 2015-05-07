using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangGuoTv
{
    public class CallbackManager
    {
        public static MainPage Mainpage{get;set;}
        public static PhoneApplicationPage currentPage { get; set; }
        public static void CallBackTrigger(int action,bool isSuccess,string type =null)
        {
            switch (action)
            {
                case (int)CommonData.CallbackType.Login:
                    //判断在哪个page调用login接口
                    if (CommonData.MainPageLoaded)
                    {
                        //Mainpage.UserLoginSuccess(isSuccess);
                        //App.ViewModel.LoginSuccess = CommonData.loginSuccess;
                    }
                  
                    break;
                case (int)CommonData.CallbackType.LoadedData:
                    if (Mainpage != null)
                    {
                        Mainpage.Dispatcher.BeginInvoke(() => 
                        {
                           // Mainpage.DataContextLoaded(isSuccess);
                        });
                    }
                    break;
                case (int)CommonData.CallbackType.LoadVideoBack:
                    if (Mainpage != null)
                    {
                       
                    }
                   
                    break;
                case (int)CommonData.CallbackType.DownVideoBack:
                    //DbFMCommonData.DownLoadedSong = true;
                    //if (musicPage != null)
                    //{
                    //    musicPage.DownLoadSongBack(isSuccess);
                    //}
                    //else if (Mainpage != null)
                    //{
                    //    Mainpage.DownSongBack(isSuccess);
                    //}
                    break;
               
                case (int)CommonData.CallbackType.OperationBack:
                 
                    break;
                default:
                    break;
            }
        }
    }
}
