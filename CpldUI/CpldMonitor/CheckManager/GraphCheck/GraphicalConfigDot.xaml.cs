using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using CpldUI.Check.Base;

namespace CpldUI.Check.GraphCheck
{
    /// <summary>
    /// GraphicalConfigDot.xaml 的交互逻辑
    /// </summary>
    public partial class GraphicalConfigDot : Window
    {
        public Dot CurDot { get; set; }
    
        public GraphicalConfigDot(Dot dot)
        {
            InitializeComponent();
            CurDot = dot;

            TbName.Text = CurDot.Info.Name;
            TbPhyAddr.Text = CurDot.Info.PhyAddr;
            TbZoomX.Text = CurDot.Info.Position.X.ToString(CultureInfo.InvariantCulture);
            TbZoomY.Text = CurDot.Info.Position.Y.ToString(CultureInfo.InvariantCulture);
        }

        private void TbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CurDot.Info.Name = TbName.Text;
            CurDot.Info.PhyAddr = TbPhyAddr.Text.Trim();
        }
    }
}
