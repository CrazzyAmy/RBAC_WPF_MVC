using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataAccessLibrary.Repository;

namespace WpfApp
{
    /// <summary>
    /// RegistratorWindow.xaml 的互動邏輯
    /// </summary>
    public partial class RegistratorWindow : Window
    {
        public RegistratorWindow()
        {
            InitializeComponent();
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if (AC.Text == null || PW.Text == null)
            {
                MessageBox.Show("帳號或密碼為空，請輸入帳號或密碼", "Error",MessageBoxButton.OK);
            }else
            {
                string t = DataAccessLibrary.Repository.MemberRepository.IdentifyAccExitOrNot(AC.Text,PW.Text);
            }
        }

        
    }
}
