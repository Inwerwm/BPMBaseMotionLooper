using System;

namespace MotionLooper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel(msg =>
            {
                TextBoxLog.AppendText(msg + Environment.NewLine);
                TextBoxLog.ScrollToEnd();
            });
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
