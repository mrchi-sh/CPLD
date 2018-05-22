using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CpldBase;
using CpldDB;
using CpldLog;
using CpldUI.Check.Bar;
using CpldUI.Check.GraphCheck;

namespace CpldUI.CheckManager.GraphCheck
{
    public class GraphicalMangementCableViewModel
    {
        public ObservableCollection<CableInfo> CableList { get; set; }
        public CableInfo SelectedCable { get; set; }
        public bool IsCableEditEnable { get; set; }
        public bool IsBarEditEnable { get; set; }
        public string SearchText { get; set; }

        private readonly UserInfo _user;

        public GraphicalMangementCableViewModel()
        {

        }

        public GraphicalMangementCableViewModel(UserInfo user)
        {
            _user = user;
            
            IsCableEditEnable = _user.UserLevel != 1;
            IsBarEditEnable = _user.UserAuthen.Contains("7") || _user.UserAuthen.Contains("5");
            CableList = new ObservableCollection<CableInfo>();
            UpdataCableGrid();
        }

        private bool UpdataCableGrid()
        {
            List<CableInfo> cableList;
            if (!CacheGraph.GetCables(out cableList))
            {
                LogControl.LogError("读取样线缓存失败");
                return false;
            }
            
            CableList.Clear();
            foreach (var cable in cableList)
            {
                CableList.Add(cable);
            }
            return true;
        }

        public bool SelectCheckCable()
        {
            if (SelectedCable == null)
            {
                InfoBox.InfoMsg("请选择产品");
                return false;
            }

            var pm = new GraphicalCableCheck(SelectedCable, _user);
            pm.Show();
            return true;
        }

        public bool SearchCable()
        {
            List<CableInfo> cableList;
            if (!CacheGraph.GetCablesByName(out cableList, SearchText))
            {
                LogControl.LogError("搜索样线缓存失败");
                return false;
            }
            CableList.Clear();
            foreach (var cable in cableList)
            {
                CableList.Add(cable);
            }
            return true;
        }

        public bool BarEdit()
        {
            if (SelectedCable==null)
            {
                InfoBox.InfoMsg("请选择产品");
                return false;
            }
            var bm = new BarMgr(SelectedCable);
            bm.ShowDialog();
            return true;
        }

        public bool DelCable()
        {
            if (SelectedCable == null)
            {
                InfoBox.InfoMsg("请选择产品");
                return false;
            }
            var mbr = MessageBox.Show("确定要删除选中的记录吗？", "提示信息", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (mbr != MessageBoxResult.OK) return true;
            if (!CacheGraph.DelCable(SelectedCable))
            {
                LogControl.LogError("删除样线缓存失败");
                return false;
            }
            if (!CacheGraph.CacheDelCable())
            {
                LogControl.LogError("写入样线数据库失败");
                return false;
            }
            CpldControl.Bartend.BartendControl.DelBartendFiles(SelectedCable.CableName);
            return UpdataCableGrid(); ;
        }

        public bool CopyCable()
        {
            if (SelectedCable == null)
            {
                InfoBox.InfoMsg("请选择产品");
                return false;
            }
          
            var mbr = MessageBox.Show("确定要复制选中的记录吗？", "提示信息", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (mbr != MessageBoxResult.OK) return true;
            if (!CacheGraph.CopyCable(SelectedCable, _user.UserName))
            {
                InfoBox.ErrorMsg("数据库访问失败，请联系管理员");
                return false;
            }
            if (!CacheGraph.CacheCopyCable())
            {
                LogControl.LogError("写入样线数据库失败");
                return false;
            }
            return UpdataCableGrid(); 
        }

        public bool AddCable()
        {
            var dpmHandler = new GraphicalConfigCableInfo(_user);
            dpmHandler.ShowDialog();
            return UpdataCableGrid();
        }

        public bool EditCable()
        {
            if (SelectedCable == null)
            {
                InfoBox.InfoMsg("请选择产品");
                return false;
            }

            var dpmHandler = new GraphicalConfigCableInfo(SelectedCable, _user);
            dpmHandler.ShowDialog();
            return UpdataCableGrid();
        }

        public bool DrawCable()
        {
            if (SelectedCable == null)
            {
                InfoBox.InfoMsg("请选择产品");
                return false;
            }

            var pm = new GraphicalConfigCable(SelectedCable, _user);
            pm.ShowDialog();
            return true;
        }
    }

    /// <summary>
    /// GraphicalMangementCable.xaml 的交互逻辑
    /// </summary>
    public partial class GraphicalMangementCable : Window
    {
        private readonly GraphicalMangementCableViewModel _viewModel;

        public GraphicalMangementCable(UserInfo user)
        {
            InitializeComponent();
            _viewModel = new GraphicalMangementCableViewModel(user);
            Grid.DataContext = _viewModel;
        }

        private void gridSampleSelect_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.SelectCheckCable();
        }
        
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectCheckCable();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SearchCable();
        }

        private void btnBarEdit_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.BarEdit();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DelCable();
        }
        
        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CopyCable();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddCable();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.EditCable();
        }

        private void btnDraw_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DrawCable();
        }
    }
}
