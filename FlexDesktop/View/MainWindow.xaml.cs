using System;
using System.Windows;
using FlexDesktop.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using FlexDesktop.Messages;
using System.Windows.Threading;

namespace FlexDesktop.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dispatcherTimer;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            

            Messenger.Default.Register<AddTorrentShowDialog>(this, OpenAddTorrentWindow);

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            lst.Items.Refresh();
        }

        private void OpenAddTorrentWindow(AddTorrentShowDialog obj)
        {
            AddTorrentWindow window = new AddTorrentWindow(obj.PathToTorrentFile);
            window.Owner = this;
            obj.CallBack(window.ShowDialog());
            dispatcherTimer.Start();
        }
    }
}