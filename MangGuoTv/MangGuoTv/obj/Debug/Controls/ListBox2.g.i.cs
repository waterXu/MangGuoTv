﻿#pragma checksum "C:\Users\xyl\Documents\Visual Studio 2013\Projects\MangGuoTv\MangGuoTv\Controls\ListBox2.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "76EE497DDF27B76E36E5F3BF273CBC76"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34209
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using MangGuoTv.Controls;
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


namespace MangGuoTv.Controls {
    
    
    public partial class ListBox2 : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.UserControl @this;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal MangGuoTv.Controls.ListBox2TopPanel topPanel;
        
        internal MangGuoTv.Controls.ListBox2MainPanel mainPanel;
        
        internal MangGuoTv.Controls.ListBox2BottomPanel bottomPanel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/MangGuoTv;component/Controls/ListBox2.xaml", System.UriKind.Relative));
            this.@this = ((System.Windows.Controls.UserControl)(this.FindName("this")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.topPanel = ((MangGuoTv.Controls.ListBox2TopPanel)(this.FindName("topPanel")));
            this.mainPanel = ((MangGuoTv.Controls.ListBox2MainPanel)(this.FindName("mainPanel")));
            this.bottomPanel = ((MangGuoTv.Controls.ListBox2BottomPanel)(this.FindName("bottomPanel")));
        }
    }
}
