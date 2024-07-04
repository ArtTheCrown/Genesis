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
        public MainWindow()
        {
            InitializeComponent();

            ImageBrush imageBrush = new ImageBrush();


            imageBrush.ImageSource = new BitmapImage(new Uri("https://cdn.discordapp.com/attachments/1234945024387715155/1254695162391564290/image.png?ex=6684f96d&is=6683a7ed&hm=b9c5089d3578a42be17ca11f7fa5032bdb2336fbb37511f61dd7683a398dcf44&", UriKind.RelativeOrAbsolute));


           _profilePicture.Background = imageBrush;
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

        public async Task SetUserProfile(User user)
        {
            // Ensure the UI update happens on the UI thread
            await _profilePicture.Dispatcher.Invoke(async () =>
            {
                try
                {
                    // Set the display name and username
                    _displayName.Text = user.Username;
                    _username.Text = $"@{user.UserID}";

                    // Determine the image URL
                    string imageUrl = user.Pfp == "null" ?
                        "https://cdn.discordapp.com/attachments/1072177095532347512/1084459972370382899/image.png?ex=6683f90d&is=6682a78d&hm=1fd306e2cd1ba0b0579682573cd4e0deb72c22411e54166f18783e8e44bb1e50&" :
                        user.Pfp;
                    Console.WriteLine(imageUrl);

                    // Download the image
                    string fileName = "profile_image.png";
                    bool downloadSuccess = await DBEngineSupport.DownloadFileAsync(imageUrl, fileName);

                    if (downloadSuccess)
                    {
                        // Set the profile picture background
                        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", fileName);
                        ImageBrush imageBrush = new ImageBrush();
                        imageBrush.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                        _profilePicture.Background = imageBrush;
                    }
                    else
                    {
                        // Optionally, handle download failure (e.g., set a default background)
                        Debug.WriteLine("Failed to download the profile picture.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error setting profile picture background: " + ex.Message);
                    // Optionally, set a default background or handle the error appropriately
                }
            });
            await Task.Delay(TimeSpan.FromMilliseconds(300));
        }

        private async void LoginSubmit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (!String.IsNullOrWhiteSpace(LoginUsernameBox.Text) && !String.IsNullOrWhiteSpace(LoginPasswordBox.Password))
            {
                StartRotation();
                LoginSubmit.Visibility = Visibility.Collapsed;
                RotatingBorder.Visibility = Visibility.Visible;
                var result = await Program.Login(LoginUsernameBox.Text, LoginPasswordBox.Password);

                if(result.Item2 != null)
                {
                    if (result.Item2.Authentication.status == "Success")
                    {
                        var user = result.Item2.Authentication.User;

                        await SetUserProfile(user);

                        _login_register.Visibility = Visibility.Collapsed;
                        _Apps.Visibility = Visibility.Visible;

                        LoginSubmit.Visibility = Visibility.Visible;
                        RotatingBorder.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        LoginSubmit.Visibility = Visibility.Visible;
                        RotatingBorder.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    LoginSubmit.Visibility = Visibility.Visible;
                    RotatingBorder.Visibility = Visibility.Collapsed;
                }

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

        private async void RegisterSubmit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(LoginUsernameBox.Text) && !String.IsNullOrWhiteSpace(LoginPasswordBox.Password))
            {
                StartRotation();
                LoginSubmit.Visibility = Visibility.Collapsed;
                RotatingBorder.Visibility = Visibility.Visible;
                var result = await Program.Login(LoginUsernameBox.Text, LoginPasswordBox.Password);

                if (result.Item2.Authentication.status == "Success")
                {
                    var user = result.Item2.Authentication.User;

                    SetUserProfile(user);

                    _login_register.Visibility = Visibility.Collapsed;
                    _Apps.Visibility = Visibility.Visible;

                    LoginSubmit.Visibility = Visibility.Visible;
                    RotatingBorder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    LoginSubmit.Visibility = Visibility.Visible;
                    RotatingBorder.Visibility = Visibility.Collapsed;
                }

            }
            else if (String.IsNullOrWhiteSpace(RegisterUsernameBox.Text) && String.IsNullOrWhiteSpace(RegisterPasswordBox.Password))
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