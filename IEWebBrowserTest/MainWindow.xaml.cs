using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GDAccountAutoGenerate;
using IEWebBrowserOperator;

namespace IEWebBrowserTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private WebBrowserUtility _wbUtility;
        private async void NavBtn_Click(object sender, RoutedEventArgs e)
        {
            _wbUtility = new WebBrowserUtility(PageBrowser);
            string result = await _wbUtility.NavigateAsync(UrlTxt.Text);
        }

        private void TestBtn_OnClick(object sender, RoutedEventArgs e)
        {
            GdAccountAutoGenerate autoGenerate = new GdAccountAutoGenerate(PageBrowser);
            autoGenerate.RegisterCard(new GdCard("4250320000170143","857"));


        }
    }
}
