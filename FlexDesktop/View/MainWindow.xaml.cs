using System;
using System.Windows;
using FlexDesktop.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using FlexDesktop.Messages;

namespace FlexDesktop.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            

            Messenger.Default.Register<AddTorrentShowDialog>(this, OpenAddTorrentWindow);
        }

        private void OpenAddTorrentWindow(AddTorrentShowDialog obj)
        {
            AddTorrentWindow window = new AddTorrentWindow(obj.PathToTorrentFile);
            window.Owner = this;
            obj.CallBack(window.ShowDialog());
        }
    }
}