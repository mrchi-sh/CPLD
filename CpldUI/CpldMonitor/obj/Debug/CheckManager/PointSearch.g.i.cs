﻿#pragma checksum "..\..\..\CheckManager\PointSearch.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E90490D2BA608F4F5EF62798276E3EFF4BF9A44B"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace CpldUI.CheckManager {
    
    
    /// <summary>
    /// PointSearch
    /// </summary>
    public partial class PointSearch : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\CheckManager\PointSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CheckRepeat;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\CheckManager\PointSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TbPointNo;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\CheckManager\PointSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer SvPoint;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\CheckManager\PointSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox LbPoint;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\CheckManager\PointSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnReset;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\CheckManager\PointSearch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnStart;
        
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
            System.Uri resourceLocater = new System.Uri("/CpldUI;component/checkmanager/pointsearch.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\CheckManager\PointSearch.xaml"
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
            
            #line 6 "..\..\..\CheckManager\PointSearch.xaml"
            ((CpldUI.CheckManager.PointSearch)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.CheckRepeat = ((System.Windows.Controls.CheckBox)(target));
            
            #line 37 "..\..\..\CheckManager\PointSearch.xaml"
            this.CheckRepeat.Click += new System.Windows.RoutedEventHandler(this.checkRepeat_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TbPointNo = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.SvPoint = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 5:
            this.LbPoint = ((System.Windows.Controls.ListBox)(target));
            return;
            case 6:
            this.BtnReset = ((System.Windows.Controls.Button)(target));
            
            #line 82 "..\..\..\CheckManager\PointSearch.xaml"
            this.BtnReset.Click += new System.Windows.RoutedEventHandler(this.Reset_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.BtnStart = ((System.Windows.Controls.Button)(target));
            
            #line 90 "..\..\..\CheckManager\PointSearch.xaml"
            this.BtnStart.Click += new System.Windows.RoutedEventHandler(this.btnStart_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

