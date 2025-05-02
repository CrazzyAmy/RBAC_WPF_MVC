using DataAccessLibrary.Repository;
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
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// LoginWindow.xaml 的互動邏輯
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly string _dbConnStr;
        public LoginWindow()
        {
            InitializeComponent();
            _dbConnStr = ((App)Application.Current).DbConnStr;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            RegistratorWindow w = new RegistratorWindow();
            w.Show();
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            MemberRepository mr = new MemberRepository(_dbConnStr);
            string result = mr.IdentifyLogin(this.AC.Text, this.PW.Password);
            if (result == "Success")
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("帳號或密碼錯誤");
            }
        }
    }
}
