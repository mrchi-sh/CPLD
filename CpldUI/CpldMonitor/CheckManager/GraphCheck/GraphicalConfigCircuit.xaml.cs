using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CpldDB;
using CpldLog;
using ComboBox = System.Windows.Controls.ComboBox;
using RadioButton = System.Windows.Controls.RadioButton;


namespace CpldUI.CheckManager.GraphCheck
{
    public class GraphicalConfigCircuitViewModel
    {

    }

    /// <summary>
    /// GraphicalConfigCircuit.xaml 的交互逻辑
    /// </summary>

    public partial class GraphicalConfigCircuit : Window
    {
        public CircuitInfo CurCircuit { get; set; }

        public GraphicalConfigCircuit(CircuitInfo circuit)
        {
            InitializeComponent();

            CmbLineColors.ItemsSource = typeof(Colors).GetProperties();
            CmbDotFgColors.ItemsSource = typeof(Colors).GetProperties();
            CmbDotBgColors.ItemsSource = typeof(Colors).GetProperties();

            CmbLineColors.ItemsSource = typeof(Colors).GetProperties();
            CmbDotFgColors.ItemsSource = typeof(Colors).GetProperties();
            CmbDotBgColors.ItemsSource = typeof(Colors).GetProperties();

            CurCircuit = circuit;

            TbCircuitName.Text = circuit.Name;
            TbRemarks.Text = circuit.Remark;

            switch (circuit.DotStyle.Type)
            {
                case 1:
                    DotType1.IsChecked = true;
                    break;
                case 2:
                    DotType2.IsChecked = true;
                    break;
                case 3:
                    DotType3.IsChecked = true;
                    break;
                default:
                    DotType1.IsChecked = true;
                    break;
            }


            switch (circuit.LineStyle.Type)
            {
                case 1:
                    LineType1.IsChecked = true;
                    break;
                case 2:
                    LineType2.IsChecked = true;
                    break;
                case 3:
                    LineType3.IsChecked = true;
                    break;
                default:
                    LineType1.IsChecked = true;
                    break;
            }

            foreach (var item in typeof(Colors).GetProperties())
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(item.Name);
                    if (color == circuit.LineStyle.ForegroundColor)
                    {
                        CmbLineColors.SelectedItem = typeof(Colors).GetProperty(item.Name);
                    }
                    if (color == circuit.DotStyle.ForegroundColor)
                    {
                        CmbDotFgColors.SelectedItem = typeof(Colors).GetProperty(item.Name);
                    }
                    if (color == circuit.DotStyle.BackgroundColor)
                    {
                        CmbDotBgColors.SelectedItem = typeof(Colors).GetProperty(item.Name);
                    }
                }
                catch (Exception e)
                {
                    LogControl.LogError(e);
                }
            }

            CmbLineWidth.SelectedIndex = Convert.ToInt32(circuit.LineStyle.Size) - 1;
            TbDotSize.Text = circuit.DotStyle.Size.ToString(CultureInfo.InvariantCulture);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb == null)
                return;
            switch (rb.Name)
            {
                case "DotType1":
                    CurCircuit.DotStyle.Type = 1;
                    break;
                case "DotType2":
                    CurCircuit.DotStyle.Type = 2;
                    break;
                case "DotType3":
                    CurCircuit.DotStyle.Type = 3;
                    break;
                case "LineType1":
                    CurCircuit.LineStyle.Type = 1;
                    break;
                case "LineType2":
                    CurCircuit.LineStyle.Type = 2;
                    break;
                case "LineType3":
                    CurCircuit.LineStyle.Type = 3;
                    break;
                default: break;
            }
        }

        private void cmbColors_ContextMenuClosing(object sender, EventArgs e)
        {
            var cmbColor = sender as ComboBox;
            if (cmbColor == null || cmbColor.SelectedItem == null)
                return;
            var tmp = cmbColor.SelectedItem as PropertyInfo;
            if (tmp == null)
                return;

            var selectedColor = (Color)tmp.GetValue(null, null);
            switch (cmbColor.Name)
            {
                case "CmbLineColors":
                    CurCircuit.LineStyle.ForegroundColor = selectedColor;
                    break;
                case "CmbDotBgColors":
                    CurCircuit.DotStyle.BackgroundColor = selectedColor;
                    break;
                case "CmbDotFgColors":
                    CurCircuit.DotStyle.ForegroundColor = selectedColor;
                    break;
            }
        }

        private void cmbLineWidth_DropDownClosed(object sender, EventArgs e)
        {
            var cmbLineWidthTmp = sender as ComboBox;
            if (cmbLineWidthTmp == null)
                return;
            var selected = cmbLineWidthTmp.Text;
            CurCircuit.LineStyle.Size = Convert.ToDouble(selected.Substring(0, 1));
           
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void tbCircuitName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            CurCircuit.Name = TbCircuitName.Text;
        }

        private void tbRemarks_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            CurCircuit.Remark = TbRemarks.Text;
        }

        private void TbDotSize_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            CurCircuit.DotStyle.Size = Convert.ToDouble(TbDotSize.Text);
        }


    }
    

}
