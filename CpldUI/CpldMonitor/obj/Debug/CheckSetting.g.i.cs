﻿#pragma checksum "..\..\CheckSetting.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "92748E9AB8BCF8188B1DFD4D1DB95C3D71C2DBA0"
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


namespace CpldUI {
    
    
    /// <summary>
    /// CheckSetting
    /// </summary>
    public partial class CheckSetting : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\CheckSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridModeSelect;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\CheckSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbManual;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\CheckSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbOkWait;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\CheckSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbOkManual;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\CheckSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbOkAuto;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\CheckSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridNgSelect;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\CheckSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbNgManual;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\CheckSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbNgAuto;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\CheckSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
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
            System.Uri resourceLocater = new System.Uri("/CpldUI;component/checksetting.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\CheckSetting.xaml"
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
            
            #line 4 "..\..\CheckSetting.xaml"
            ((CpldUI.CheckSetting)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.gridModeSelect = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.rbManual = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 4:
            this.rbOkWait = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.rbOkManual = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 6:
            this.rbOkAuto = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 7:
            this.gridNgSelect = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.rbNgManual = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 9:
            this.rbNgAuto = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 10:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\CheckSetting.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.btnOk_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
