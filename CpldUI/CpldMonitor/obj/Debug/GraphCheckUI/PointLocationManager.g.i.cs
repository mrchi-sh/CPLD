﻿#pragma checksum "..\..\..\GraphCheckUI\PointLocationManager.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "51FE33FBC6F21B430E5BA76965AFABD1A406CC4C"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using CpldUI.GraphCheckUI;
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


namespace CpldUI.GraphCheckUI {
    
    
    /// <summary>
    /// PointLocationManager
    /// </summary>
    public partial class PointLocationManager : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 33 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas canvasDraw;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgBackGround;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblPointInfo;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridPointList;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvPoint;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnImportBackGround;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClearPoints;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDynPointCheckBegin;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDynPointCheckStop;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblConnPoint;
        
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
            System.Uri resourceLocater = new System.Uri("/CpldUI;component/graphcheckui/pointlocationmanager.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
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
            
            #line 5 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            ((CpldUI.GraphCheckUI.PointLocationManager)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 5 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            ((CpldUI.GraphCheckUI.PointLocationManager)(target)).ContentRendered += new System.EventHandler(this.Window_ContentRendered);
            
            #line default
            #line hidden
            
            #line 5 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            ((CpldUI.GraphCheckUI.PointLocationManager)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 3:
            this.canvasDraw = ((System.Windows.Controls.Canvas)(target));
            
            #line 33 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            this.canvasDraw.SizeChanged += new System.Windows.SizeChangedEventHandler(this.canvasDraw_SizeChanged);
            
            #line default
            #line hidden
            
            #line 33 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            this.canvasDraw.Drop += new System.Windows.DragEventHandler(this.canvasDraw_Drop);
            
            #line default
            #line hidden
            return;
            case 4:
            this.imgBackGround = ((System.Windows.Controls.Image)(target));
            
            #line 34 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            this.imgBackGround.SizeChanged += new System.Windows.SizeChangedEventHandler(this.imgBackGround_SizeChanged);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            this.imgBackGround.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.imgBackGround_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            this.imgBackGround.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.imgBackGround_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.lblPointInfo = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.gridPointList = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.lvPoint = ((System.Windows.Controls.ListView)(target));
            
            #line 51 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            this.lvPoint.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lvPoint_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnImportBackGround = ((System.Windows.Controls.Button)(target));
            
            #line 72 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            this.btnImportBackGround.Click += new System.Windows.RoutedEventHandler(this.btnImportBackGround_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnClearPoints = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            this.btnClearPoints.Click += new System.Windows.RoutedEventHandler(this.btnClearPoints_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnDynPointCheckBegin = ((System.Windows.Controls.Button)(target));
            
            #line 78 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            this.btnDynPointCheckBegin.Click += new System.Windows.RoutedEventHandler(this.btnDynPointCheckBegin_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.btnDynPointCheckStop = ((System.Windows.Controls.Button)(target));
            
            #line 79 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            this.btnDynPointCheckStop.Click += new System.Windows.RoutedEventHandler(this.btnDynPointCheckStop_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.lblConnPoint = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 2:
            
            #line 8 "..\..\..\GraphCheckUI\PointLocationManager.xaml"
            ((System.Windows.Controls.Grid)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Grid_Loaded);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
