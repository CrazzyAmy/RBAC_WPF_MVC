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
        private readonly string _dbConnStr;
        public RegistratorWindow()
        {
            InitializeComponent();
            _dbConnStr = ((App)Application.Current).DbConnStr;
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.AC.Text == null || this.PW.Password == null)
            {
                MessageBox.Show("帳號或密碼為空，請輸入帳號或密碼", "Error",MessageBoxButton.OK);
            }else
            {
                MemberRepository mr = new MemberRepository(_dbConnStr);
                string t = mr.IdentifyAccExitOrNot(this.AC.Text,this.PW.Password);
                if(t == "Had")
                {
                    MessageBox.Show("帳號已存在");
                }
                else
                {
                    if (this.RPW.Password != null)
                    {
                        if (this.PW.Password == this.RPW.Password)
                        {
                            mr.Registrator(this.AC.Text, this.PW.Password);
                            MessageBox.Show("註冊成功");
                            MainWindow win2 = new MainWindow();
                            win2.Show();
                        }
                        else
                        {
                            MessageBox.Show("密碼不一致");
                        }
                        
                    }
                }
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow lw = new LoginWindow();
            lw.Show();
        }
    }
}
