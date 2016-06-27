using FlexDesktop.Messages;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System;
using System.Windows.Forms;
using FlexDesktop.ViewModel;

namespace FlexDesktop.View
{
    public partial class AddTorrentWindow : Window
    {
        public AddTorrentWindow(string pathToTorrentFile)
        {
            InitializeComponent();

            Closing += AddTorrentWindow_Closing;

            Messenger.Default.Register<FolderBrowserShowDialogMessage>(this, FolderBrowserShowDialogHandler);
            Messenger.Default.Register<string>(this, CloseWindow);

            ((AddTorrentWindowViewModel)DataContext).PathToTorrentFile = pathToTorrentFile;
        }

        private void AddTorrentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Messenger.Default.Unregister<FolderBrowserShowDialogMessage>(this, FolderBrowserShowDialogHandler);
            Messenger.Default.Unregister<string>(this, CloseWindow);
        }

        private void CloseWindow(string obj)
        {
            if (obj.Equals("AddTorrentOkCommand"))
                DialogResult = true;
            else if(obj.Equals("AddTorrentCancelCommand"))
                DialogResult = false;
        }

        private void FolderBrowserShowDialogHandler(FolderBrowserShowDialogMessage obj)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Выберите папку для загрузки";
            folderBrowser.ShowNewFolderButton = true;
            DialogResult result = folderBrowser.ShowDialog();

            obj.CallBack(folderBrowser.SelectedPath, result == System.Windows.Forms.DialogResult.OK ? true : false);
        }
        
    }
}