using System.Windows;
using System.Windows.Input;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for HostFrame.xaml
    /// </summary>
    public partial class HostFrame : Window
    {
        public HostFrame()
        {
            InitializeComponent();
            if (CustomControl is ReportViewer)
            {
                this.AllowsTransparency = false;
            }
        }

        private void MinimizeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void CloseButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}