﻿#pragma checksum "..\..\..\Check\StandardCheckSelect.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "471BF1CE4312A88216DA93F7963D85F7C3A32100"
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


namespace CpldUI.Check {
    
    
    /// <summary>
    /// StandardCheckSelect
    /// </summary>
    public partial class StandardCheckSelect : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\Check\StandardCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox groupBox1;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Check\StandardCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid gridSampleSelect;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\Check\StandardCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbProdName;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\Check\StandardCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Check\StandardCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Check\StandardCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBarEdit;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\Check\StandardCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAdd;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\Check\StandardCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDel;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\Check\StandardCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBarMatch;
        
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
            System.Uri resourceLocater = new System.Uri("/CpldUI;component/check/standardcheckselect.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Check\StandardCheckSelect.xaml"
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
            
            #line 4 "..\..\..\Check\StandardCheckSelect.xaml"
            ((CpldUI.Check.StandardCheckSelect)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\Check\StandardCheckSelect.xaml"
            ((CpldUI.Check.StandardCheckSelect)(target)).Activated += new System.EventHandler(this.Window_Activated);
            
            #line default
            #line hidden
            return;
            case 2:
            this.groupBox1 = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 3:
            this.gridSampleSelect = ((System.Windows.Controls.DataGrid)(target));
            
            #line 33 "..\..\..\Check\StandardCheckSelect.xaml"
            this.gridSampleSelect.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.gridSampleSelect_KeyDown);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\Check\StandardCheckSelect.xaml"
            this.gridSampleSelect.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.gridSampleSelect_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tbProdName = ((System.Windows.Controls.TextBox)(target));
            
            #line 49 "..\..\..\Check\StandardCheckSelect.xaml"
            this.tbProdName.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.tbProdName_TextChanged);
            
            #line default
            #line hidden
            
            #line 49 "..\..\..\Check\StandardCheckSelect.xaml"
            this.tbProdName.KeyUp += new System.Windows.Input.KeyEventHandler(this.tbProdName_KeyUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnSearch = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\Check\StandardCheckSelect.xaml"
            this.btnSearch.Click += new System.Windows.RoutedEventHandler(this.btnSearch_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 58 "..\..\..\Check\StandardCheckSelect.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.btnOk_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnBarEdit = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\Check\StandardCheckSelect.xaml"
            this.btnBarEdit.Click += new System.Windows.RoutedEventHandler(this.btnBarEdit_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnAdd = ((System.Windows.Controls.Button)(target));
            
            #line 71 "..\..\..\Check\StandardCheckSelect.xaml"
            this.btnAdd.Click += new System.Windows.RoutedEventHandler(this.btnAdd_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnDel = ((System.Windows.Controls.Button)(target));
            
            #line 77 "..\..\..\Check\StandardCheckSelect.xaml"
            this.btnDel.Click += new System.Windows.RoutedEventHandler(this.btnDel_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnBarMatch = ((System.Windows.Controls.Button)(target));
            
            #line 79 "..\..\..\Check\StandardCheckSelect.xaml"
            this.btnBarMatch.Click += new System.Windows.RoutedEventHandler(this.btnBarMatch_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

