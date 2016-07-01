using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System;
using System.ComponentModel;

namespace FlexDesktop.View
{
    /// <summary>
    /// Description for DeleteTorrentWindow.
    /// </summary>
    public partial class DeleteTorrentWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the DeleteTorrentWindow class.
        /// </summary>
        public DeleteTorrentWindow()
        {
            InitializeComponent();

            Closing += DeleteTorrentWindow_Closing;

            Messenger.Default.Register<string>(this, CloseWindow);
        }

        private void DeleteTorrentWindow_Closing(object sender, CancelEventArgs e)
        {
            Messenger.Default.Unregister<string>(this, CloseWindow);
        }

        private void CloseWindow(string message)
        {
            if (message.Equals("DeleteTorrentOkCommand"))
                DialogResult = true;
            else
                DialogResult = false;
        }
    }
}