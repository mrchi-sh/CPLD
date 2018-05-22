using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CpldUI
{
    /// <summary>
    /// IPTextBoxCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class IpTextBoxCtrl : UserControl
    {
        public IpTextBoxCtrl()
        {
            InitializeComponent();
            SetUndoLimit();
            Text = DefaultIp;
        }
        #region Field and Property
        private string _backIp = string.Empty;
        private string _ip = string.Empty;
        private const string DefaultIp = "127.0.0.1";

        public string Text
        {
            get
            {
                return string.Format("{0}.{1}.{2}.{3}",
                    TbFirstIp.Text.Length != 0 ? TbFirstIp.Text : "0",
                    TbSecondIp.Text.Length != 0 ? TbSecondIp.Text : "0",
                    TbThirdIp.Text.Length != 0 ? TbThirdIp.Text : "0",
                    TbForthIp.Text.Length != 0 ? TbForthIp.Text : "0");
            }
            set { SetIp(IsIpString(value) ? value : DefaultIp); }
        }
        #endregion

        #region Base Method
        private static bool CheckIntegerNumber(string strNumber, int length)
        {
            if (string.IsNullOrWhiteSpace(strNumber) || strNumber.Length > length)
                return false;
            return strNumber.All(c => c >= '0' && c <= '9');
        }
        private static string RemoveFirstZero(string strNumber)
        {
            if (string.IsNullOrWhiteSpace(strNumber))
                return string.Empty;
            strNumber = strNumber.Trim();
            while (true)
            {
                if (strNumber.Length > 1 && strNumber[0] == '0')
                {
                    strNumber = strNumber.Substring(1, strNumber.Length - 1);
                }
                else
                {
                    if (strNumber.Length == 1 && strNumber == "0")
                    {
                        return "0";
                    }
                    break;
                }
            }
            return strNumber;
        }
        private static bool IsIpString(string iPString)
        {
            if (string.IsNullOrWhiteSpace(iPString))
            {
                return false;
            }
            var split = iPString.Split('.');
            if (split.Length != 4)
                return false;
            foreach (var strNumber in split)
            {
                if (CheckIntegerNumber(strNumber, 3))
                {
                    if (int.Parse(strNumber) > 255)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Event
        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (InputMethod.Current != null) InputMethod.Current.ImeState = InputMethodState.Off;
            var tb = sender as TextBox;
            if (tb != null && tb.Text.Length != 0)
            {
                tb.SelectAll();
            }
        }
        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab) return;
            var tb = sender as TextBox;
            if (e.Key == Key.OemPeriod)
            {
                e.Handled = true;
                SetNextFocus(tb);
                return;
            }
            if (e.Key < Key.D0 || e.Key > Key.D9 && e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
            {
                e.Handled = true;
                return;
            }
            if (tb != null && (tb.Text.Length != 3 || tb.SelectedText.Length != 0)) return;
            e.Handled = true;
        }
        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.Text.Length == 0)
                return;
            var strNumber = RemoveFirstZero(tb.Text);
            if (CheckIntegerNumber(strNumber, 3))
            {
                if (int.Parse(strNumber) <= 255)
                {
                    _backIp = strNumber;
                }
            }
            tb.Text = _backIp;
            SetNextFocus(tb, false);
        }
        private void TextBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key != Key.V) || !Keyboard.IsKeyDown(Key.LeftCtrl)) return;
            if (!Clipboard.ContainsText()) return;
            var clipboardString = Clipboard.GetText();
            if (!IsIpString(clipboardString)) return;
            SetIp(clipboardString);
            e.Handled = true;
        }
        #endregion

        #region Other Logic Method
        private void MoveNextTextBox(string tbName)
        {
            switch (tbName)
            {
                case "tbFirstIP":
                    TbSecondIp.Focus();
                    TbSecondIp.SelectAll();
                    break;
                case "tbSecondIP":
                    TbThirdIp.Focus();
                    TbThirdIp.SelectAll();
                    break;
                case "tbThirdIP":
                    TbForthIp.Focus();
                    TbForthIp.SelectAll();
                    break;
                case "tbForthIP":
                    TbFirstIp.Focus();
                    TbFirstIp.SelectAll();
                    break;
                default:
                    break;
            }
        }
        private void SetNextFocus(TextBox tb, bool IsForce = true)
        {
            if (IsForce)
            {
                MoveNextTextBox(tb.Name);
            }
            else
            {
                if (tb.Text.Length == 3 || (tb.Text.Length == 2 && int.Parse(tb.Text.Substring(0, 2)) > 25))
                {
                    MoveNextTextBox(tb.Name);
                }
            }
        }
        private void SetIp(string iPString)
        {
            if (string.IsNullOrWhiteSpace(iPString))
            {
                return;
            }
            var split = iPString.Split('.');
            if (split.Length != 4)
                return;
            TbFirstIp.Text = split[0];
            TbSecondIp.Text = split[1];
            TbThirdIp.Text = split[2];
            TbForthIp.Text = split[3];
        }
        private void SetUndoLimit()
        {
            TbFirstIp.UndoLimit = 1;
            TbSecondIp.UndoLimit = 1;
            TbThirdIp.UndoLimit = 1;
            TbForthIp.UndoLimit = 1;
        }
        #endregion   
    }
}
