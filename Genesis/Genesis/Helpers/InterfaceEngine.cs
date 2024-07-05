using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genesis.Genesis;
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


namespace Genesis.Genesis.Helpers
{
    public class InterfaceEngine
    {
        public MainWindow mw;

        public InterfaceEngine(MainWindow mw)
        {
            this.mw = mw;
        }

        public void MaximizeWithoutCoveringTaskbar()
        {
            // Get the working area of the screen (excluding the taskbar)
            Rect workingArea = SystemParameters.WorkArea;

            // Set the window size and position to match the working area
            mw.Left = workingArea.Left;
            mw.Top = workingArea.Top;
            mw.Width = workingArea.Width;
            mw.Height = workingArea.Height;
        }

        public async Task SetUserProfile(User user)
        {
            // Ensure the UI update happens on the UI thread
            await mw._profilePicture.Dispatcher.Invoke(async () =>
            {
                try
                {
                    // Set the display name and username
                    mw._displayName.Text = user.Username;
                    mw._username.Text = $"@{user.UserID}";

                    // Determine the image URL
                    string imageUrl = user.Pfp == "null" ?
                        "https://cdn.discordapp.com/attachments/1072177095532347512/1084459972370382899/image.png?ex=6683f90d&is=6682a78d&hm=1fd306e2cd1ba0b0579682573cd4e0deb72c22411e54166f18783e8e44bb1e50&" :
                        user.Pfp;
                    Console.WriteLine(imageUrl);

                    // Download the image
                    string fileName = "profile_image.png";
                    bool downloadSuccess = await DBEngineSupport.DownloadFileAsync(imageUrl, fileName);

                    string download = await DBEngineSupport.DownloadAttachmentAsync(imageUrl);


                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = new BitmapImage(new Uri(download, UriKind.Absolute));
                    mw._profilePicture.Background = imageBrush;

                    /*
                    if (downloadSuccess)
                    {
                        // Set the profile picture background
                        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", fileName);
                        ImageBrush imageBrush = new ImageBrush();
                        imageBrush.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                        mw._profilePicture.Background = imageBrush;
                    }
                    else
                    {
                        // Optionally, handle download failure (e.g., set a default background)
                        Debug.WriteLine("Failed to download the profile picture.");
                    }*/
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error setting profile picture background: " + ex.Message);
                    // Optionally, set a default background or handle the error appropriately
                }
            });
            await Task.Delay(TimeSpan.FromMilliseconds(20));
        }

        public async Task LoginAsync() 
        {
            if (!String.IsNullOrWhiteSpace(mw.LoginUsernameBox.Text) && !String.IsNullOrWhiteSpace(mw.LoginPasswordBox.Password))
            {
                StartRotation();
                mw.LoginSubmit.Visibility = Visibility.Collapsed;
                mw.RotatingBorder.Visibility = Visibility.Visible;
                var result = await Program.Login(mw.LoginUsernameBox.Text, mw.LoginPasswordBox.Password);

                if (result.Item2 != null)
                {
                    if (result.Item2.Authentication.status == "Success")
                    {
                        var user = result.Item2.Authentication.User;

                        await SetUserProfile(user);

                        mw._login_register.Visibility = Visibility.Collapsed;
                        mw._Apps.Visibility = Visibility.Visible;

                        mw.LoginSubmit.Visibility = Visibility.Visible;
                        mw.RotatingBorder.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        mw.LoginSubmit.Visibility = Visibility.Visible;
                        mw.RotatingBorder.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    mw.LoginSubmit.Visibility = Visibility.Visible;
                    mw.RotatingBorder.Visibility = Visibility.Collapsed;
                }

            }
            else if (String.IsNullOrWhiteSpace(mw.LoginUsernameBox.Text) && String.IsNullOrWhiteSpace(mw.LoginPasswordBox.Password))
            {
                mw._loginUsernameRequired.Visibility = Visibility.Visible;
                mw._loginPasswordRequired.Visibility = Visibility.Visible;
            }
            else if (String.IsNullOrWhiteSpace(mw.LoginUsernameBox.Text))
            {
                mw._loginUsernameRequired.Visibility = Visibility.Visible;
            }
            else if (String.IsNullOrWhiteSpace(mw.LoginPasswordBox.Password))
            {
                mw._loginPasswordRequired.Visibility = Visibility.Visible;
            }
        }

        public async Task RegisterAsync()
        {
            if ((!String.IsNullOrWhiteSpace(mw.RegisterUsernameBox.Text)
                 && !String.IsNullOrWhiteSpace(mw.RegisterDisplayNameBox.Text)
                 && !String.IsNullOrWhiteSpace(mw.RegisterPasswordBox.Password)
                 && !String.IsNullOrWhiteSpace(mw.RegisterPasswordBoxConfirm.Password)) 
                 && (mw.RegisterPasswordBox.Password == mw.RegisterPasswordBoxConfirm.Password))
            {
                /*
                StartRotation();
                RegisterSubmit.Visibility = Visibility.Collapsed;
                RotatingBorder1.Visibility = Visibility.Visible;
                var result = await Program.Register(RegisterUsernameBox.Text, RegisterDisplayNameBox.Text, RegisterPasswordBox.Password);

                if (result.Item2.Authentication.status == "Success")
                {
                    var user = result.Item2.Authentication.User;

                    SetUserProfile(user);

                    _login_register.Visibility = Visibility.Collapsed;
                    _Apps.Visibility = Visibility.Visible;

                    RegisterSubmit.Visibility = Visibility.Visible;
                    RotatingBorder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    RegisterSubmit.Visibility = Visibility.Visible;
                    RotatingBorder1.Visibility = Visibility.Collapsed;
                }
                */

            }
            else
            {
                if (String.IsNullOrWhiteSpace(mw.RegisterUsernameBox.Text)) mw._registerUsernameRequired.Visibility = Visibility.Visible;
                if (String.IsNullOrWhiteSpace(mw.RegisterDisplayNameBox.Text)) mw._registerDisplayNameRequired.Visibility = Visibility.Visible;
                if (String.IsNullOrWhiteSpace(mw.RegisterPasswordBox.Password))
                {
                    mw._registerPasswordRequired.Text = "Required";
                    mw._registerPasswordRequired.Visibility = Visibility.Visible;
                }
                if (String.IsNullOrWhiteSpace(mw.RegisterPasswordBoxConfirm.Password))
                {
                    mw._registerPasswordRequiredConfirm.Text = "Required";
                    mw._registerPasswordRequiredConfirm.Visibility = Visibility.Visible;
                }
                if (mw.RegisterPasswordBox.Password != mw.RegisterPasswordBoxConfirm.Password)
                {
                    mw._registerPasswordRequired.Text = "Must match!";
                    mw._registerPasswordRequiredConfirm.Text = "Must match!";
                    mw._registerPasswordRequired.Visibility = Visibility.Visible;
                    mw._registerPasswordRequiredConfirm.Visibility = Visibility.Visible;
                }
            }
        }

        private void StartRotation()
        {
            Storyboard rotateStoryboard = (Storyboard)mw.Resources["RotateStoryboard"];
            rotateStoryboard.Begin();
        }
    }
}
