﻿#pragma checksum "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "D1EB80AAA1FE6EE52DA78A4B995885412A4315B7"
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
    /// SectionCheckSelect
    /// </summary>
    public partial class SectionCheckSelect : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox groupBox1;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid gridSectionSelect;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbSectionName;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBarEdit;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAdd;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEdit;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDel;
        
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
            System.Uri resourceLocater = new System.Uri("/CpldUI;component/check/sectioncheck/sectioncheckselect.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
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
            
            #line 4 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
            ((CpldUI.Check.SectionCheckSelect)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
            ((CpldUI.Check.SectionCheckSelect)(target)).Activated += new System.EventHandler(this.Window_Activated);
            
            #line default
            #line hidden
            return;
            case 2:
            this.groupBox1 = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 3:
            this.gridSectionSelect = ((System.Windows.Controls.DataGrid)(target));
            
            #line 27 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
            this.gridSectionSelect.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.gridSampleSelect_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tbSectionName = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
            this.tbSectionName.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.tbSectionName_TextChanged);
            
            #line default
            #line hidden
            
            #line 41 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
            this.tbSectionName.KeyUp += new System.Windows.Input.KeyEventHandler(this.tbSectionName_KeyUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnSearch = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
            this.btnSearch.Click += new System.Windows.RoutedEventHandler(this.btnSearch_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.btnOk_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnBarEdit = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
            this.btnBarEdit.Click += new System.Windows.RoutedEventHandler(this.btnBarEdit_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnAdd = ((System.Windows.Controls.Button)(target));
            
            #line 58 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
            this.btnAdd.Click += new System.Windows.RoutedEventHandler(this.btnAdd_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnEdit = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
            this.btnEdit.Click += new System.Windows.RoutedEventHandler(this.btnEdit_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnDel = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\..\Check\SectionCheck\SectionCheckSelect.xaml"
            this.btnDel.Click += new System.Windows.RoutedEventHandler(this.btnDel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

