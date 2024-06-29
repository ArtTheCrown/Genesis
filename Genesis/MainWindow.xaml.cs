using Genesis.Genesis;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Genesis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MaximizeWithoutCoveringTaskbar()
        {
            // Get the working area of the screen (excluding the taskbar)
            Rect workingArea = SystemParameters.WorkArea;

            // Set the window size and position to match the working area
            this.Left = workingArea.Left;
            this.Top = workingArea.Top;
            this.Width = workingArea.Width;
            this.Height = workingArea.Height;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MaximizeWithoutCoveringTaskbar();
        }

        private void LoginFormShow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoginForm.Visibility = Visibility.Visible;
            RegisterForm.Visibility = Visibility.Collapsed;
        }

        private void RegisterFormShow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoginForm.Visibility = Visibility.Collapsed;
            RegisterForm.Visibility = Visibility.Visible;
        }

        private void LoginSubmit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(LoginUsernameBox.Text) && !String.IsNullOrWhiteSpace(LoginPasswordBox.Password))
            {
                
            }else if (String.IsNullOrWhiteSpace(LoginUsernameBox.Text) && String.IsNullOrWhiteSpace(LoginPasswordBox.Password))
            {
                _loginUsernameRequired.Visibility = Visibility.Visible;
                _loginPasswordRequired.Visibility = Visibility.Visible;
            }
            else if (String.IsNullOrWhiteSpace(LoginUsernameBox.Text))
            {
                _loginUsernameRequired.Visibility = Visibility.Visible;
            }
            else if (String.IsNullOrWhiteSpace(LoginPasswordBox.Password))
            {
                _loginPasswordRequired.Visibility = Visibility.Visible;
            }
        }

        private void RegisterSubmit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}