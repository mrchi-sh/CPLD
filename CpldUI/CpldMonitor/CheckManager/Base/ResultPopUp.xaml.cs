using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Threading;

namespace CpldUI
{
    /// <summary>
    /// ResultPopUp.xaml 的交互逻辑
    /// </summary>
    public partial class ResultPopUp : Window
    {
        private readonly bool _isCheckOk;
        private readonly bool _okAutoClose;
        private readonly bool _ngAutoClose;

        public ResultPopUp()
        {
            InitializeComponent();
        }

        public ResultPopUp(bool isCheckOk, bool okAutoClose, bool ngAutoClose)
        {
            InitializeComponent();
            _isCheckOk = isCheckOk;
            _okAutoClose = okAutoClose;
            _ngAutoClose = ngAutoClose;
        }

        private void AutoClose()
        {
            var countDown = 2;
            while (countDown > 0)
            {
                countDown--;
                Thread.Sleep(1000);
            }
            Dispatcher.Invoke(new Action(Close));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isCheckOk)
            {
                ImageResult.Source = new BitmapImage(new Uri("../../Icon/ok.bmp", UriKind.RelativeOrAbsolute));
                if (_okAutoClose)
                {
                    new Thread(AutoClose).Start();
                }
            }
            else
            {
                ImageResult.Source = new BitmapImage(new Uri("../../Icon/ng.bmp", UriKind.RelativeOrAbsolute));
                if (_ngAutoClose)
                {
                    new Thread(AutoClose).Start();
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        } 
    }
}
