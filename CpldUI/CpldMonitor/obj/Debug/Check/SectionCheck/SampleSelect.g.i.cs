﻿#pragma checksum "..\..\..\..\Check\SectionCheck\SampleSelect.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "90134ADCCB92B2C54BA2AC6CCB987E33AA4343C1"
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
    /// SampleSelect
    /// </summary>
    public partial class SampleSelect : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox groupBox1;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid gridSampleSelect;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbProdName;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearch;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
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
            System.Uri resourceLocater = new System.Uri("/CpldUI;component/check/sectioncheck/sampleselect.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
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
            
            #line 4 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
            ((CpldUI.Check.SampleSelect)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
            ((CpldUI.Check.SampleSelect)(target)).Activated += new System.EventHandler(this.Window_Activated);
            
            #line default
            #line hidden
            return;
            case 2:
            this.groupBox1 = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 3:
            this.gridSampleSelect = ((System.Windows.Controls.DataGrid)(target));
            
            #line 27 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
            this.gridSampleSelect.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.gridSampleSelect_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tbProdName = ((System.Windows.Controls.TextBox)(target));
            
            #line 42 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
            this.tbProdName.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.tbProdName_TextChanged);
            
            #line default
            #line hidden
            
            #line 42 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
            this.tbProdName.KeyUp += new System.Windows.Input.KeyEventHandler(this.tbProdName_KeyUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnSearch = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
            this.btnSearch.Click += new System.Windows.RoutedEventHandler(this.btnSearch_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\..\..\Check\SectionCheck\SampleSelect.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.btnOk_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

