using System;
using System.Windows;

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

        private void MetroWindow_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        private void MetroWindow_Drop(object sender, DragEventArgs e)
        {
            ((ViewModel)DataContext).ReadFile(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
        }
    }
}
