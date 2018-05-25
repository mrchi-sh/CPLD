using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using CpldDB;
using CpldLog;
using ComboBox = System.Windows.Controls.ComboBox;


namespace CpldUI.CheckManager.GraphCheck
{

    public class TypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            var s = (int)value;
            return parameter != null && s == int.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 1;
            var isChecked = (bool)value;
            if (!isChecked)
            {
                return null;
            }
            return parameter == null ? 1 : int.Parse(parameter.ToString());
        }
    }
    public class ColorToPropertyInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
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

            CurCircuit = circuit;
            Grid.DataContext = CurCircuit;

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

    }
    

}
