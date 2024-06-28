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

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            //var response = await Program.Register(username.Text, password.Text);

            //if (response.Item1) status.Text = response.Item2?.Registration?.status;
            //else status.Text = "Failure";
            
            MaximizeWithoutCoveringTaskbar();
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
    }
}