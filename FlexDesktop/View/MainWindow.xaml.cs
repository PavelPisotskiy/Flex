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
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            

            Messenger.Default.Register<AddTorrentShowDialog>(this, OpenAddTorrentWindow);
            Messenger.Default.Register<DeleteTorrentShowDialog>(this, OpenDeleteTorrentWindow);

        }

        private void OpenDeleteTorrentWindow(DeleteTorrentShowDialog obj)
        {
            DeleteTorrentWindow window = new DeleteTorrentWindow();
            window.Owner = this;
            obj.CallBack(window.ShowDialog());
        }

        private void OpenAddTorrentWindow(AddTorrentShowDialog obj)
        {
            AddTorrentWindow window = new AddTorrentWindow(obj.PathToTorrentFile);
            window.Owner = this;
            obj.CallBack(window.ShowDialog());
        }
    }
}