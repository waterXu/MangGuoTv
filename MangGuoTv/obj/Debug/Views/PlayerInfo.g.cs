﻿#pragma checksum "C:\Users\xyl\Documents\Visual Studio 2013\Projects\MangGuoTv\MangGuoTv\Views\PlayerInfo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4F5919D90083CCB0A7BFC6F762F1526B"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34209
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace MangGuoTv.Views {
    
    
    public partial class PlayerInfo : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Pivot MainPivot;
        
        internal Microsoft.Phone.Controls.PivotItem DramaItem;
        
        internal System.Windows.Controls.ListBox AllDramas;
        
        internal Microsoft.Phone.Controls.PivotItem DetailItem;
        
        internal Microsoft.Phone.Controls.PivotItem CommentItem;
        
        internal Microsoft.Phone.Controls.PivotItem SceneItem;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/MangGuoTv;component/Views/PlayerInfo.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.MainPivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("MainPivot")));
            this.DramaItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("DramaItem")));
            this.AllDramas = ((System.Windows.Controls.ListBox)(this.FindName("AllDramas")));
            this.DetailItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("DetailItem")));
            this.CommentItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("CommentItem")));
            this.SceneItem = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("SceneItem")));
        }
    }
}

