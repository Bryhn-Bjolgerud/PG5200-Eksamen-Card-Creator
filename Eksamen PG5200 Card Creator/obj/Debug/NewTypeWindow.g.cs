﻿#pragma checksum "..\..\NewTypeWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D79B1D2F0118C6E951E1C0D1B4EAB20437935B1DE572DE0FCDB47AD939CAC46E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Eksamen_PG5200_Card_Creator;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Eksamen_PG5200_Card_Creator {
    
    
    /// <summary>
    /// NewTypeWindow
    /// </summary>
    public partial class NewTypeWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\NewTypeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel statPicker;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\NewTypeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox typeValue;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\NewTypeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox manaValue;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\NewTypeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox damageValue;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\NewTypeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox healthValue;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\NewTypeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button uploadImage;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\NewTypeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button createType;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\NewTypeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas canvas;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\NewTypeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MyGrid;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\NewTypeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image userSelectedImage;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Eksamen PG5200 Card Creator;component/newtypewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\NewTypeWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.statPicker = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.typeValue = ((System.Windows.Controls.TextBox)(target));
            
            #line 13 "..\..\NewTypeWindow.xaml"
            this.typeValue.GotFocus += new System.Windows.RoutedEventHandler(this.ResetTextbox_GotFocus);
            
            #line default
            #line hidden
            
            #line 13 "..\..\NewTypeWindow.xaml"
            this.typeValue.LostFocus += new System.Windows.RoutedEventHandler(this.TypeValue_LostFocus);
            
            #line default
            #line hidden
            return;
            case 3:
            this.manaValue = ((System.Windows.Controls.TextBox)(target));
            
            #line 14 "..\..\NewTypeWindow.xaml"
            this.manaValue.GotFocus += new System.Windows.RoutedEventHandler(this.ResetTextbox_GotFocus);
            
            #line default
            #line hidden
            
            #line 14 "..\..\NewTypeWindow.xaml"
            this.manaValue.LostFocus += new System.Windows.RoutedEventHandler(this.ManaValue_LostFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.damageValue = ((System.Windows.Controls.TextBox)(target));
            
            #line 15 "..\..\NewTypeWindow.xaml"
            this.damageValue.GotFocus += new System.Windows.RoutedEventHandler(this.ResetTextbox_GotFocus);
            
            #line default
            #line hidden
            
            #line 15 "..\..\NewTypeWindow.xaml"
            this.damageValue.LostFocus += new System.Windows.RoutedEventHandler(this.DamageValue_LostFocus);
            
            #line default
            #line hidden
            return;
            case 5:
            this.healthValue = ((System.Windows.Controls.TextBox)(target));
            
            #line 16 "..\..\NewTypeWindow.xaml"
            this.healthValue.GotFocus += new System.Windows.RoutedEventHandler(this.ResetTextbox_GotFocus);
            
            #line default
            #line hidden
            
            #line 16 "..\..\NewTypeWindow.xaml"
            this.healthValue.LostFocus += new System.Windows.RoutedEventHandler(this.HealthValue_LostFocus);
            
            #line default
            #line hidden
            return;
            case 6:
            this.uploadImage = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\NewTypeWindow.xaml"
            this.uploadImage.Click += new System.Windows.RoutedEventHandler(this.UploadImage_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.createType = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\NewTypeWindow.xaml"
            this.createType.Click += new System.Windows.RoutedEventHandler(this.CreateType_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.canvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 9:
            this.MyGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 10:
            this.userSelectedImage = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

