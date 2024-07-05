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
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using Genesis.Genesis.Helpers;
using System.Diagnostics;


namespace Genesis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InterfaceEngine interfaceEngine;

        public MainWindow()
        {
            InitializeComponent();

            interfaceEngine = new InterfaceEngine(this);
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            interfaceEngine.MaximizeWithoutCoveringTaskbar();
        }

        private async void LoginSubmit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await interfaceEngine.LoginAsync();
        }

        private async void RegisterSubmit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await interfaceEngine.RegisterAsync();
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

        private void StartRotation()
        {
            Storyboard rotateStoryboard = (Storyboard)this.Resources["RotateStoryboard"];
            rotateStoryboard.Begin();
        }

        private void RegisterUsernameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}