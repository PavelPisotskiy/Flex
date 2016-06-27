using FlexDesktop.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MonoTorrent.Common;
using System.IO;
using System.Windows.Input;
using System;
using MonoTorrent.BEncoding;

namespace FlexDesktop.ViewModel
{
    public class AddTorrentWindowViewModel : ViewModelBase
    {
        public AddTorrentWindowViewModel()
        {
            pathToFolder = @"E:\FlexTempFolder";
        }

        private string pathToFolder;

        public string PathToFolder
        {
            get { return pathToFolder; }
            set
            {
                pathToFolder = value;
                RaisePropertyChanged(() => PathToFolder);
                okCommand.RaiseCanExecuteChanged();
            }
        }

        private string pathToTorrentFile;

        public string PathToTorrentFile
        {
            get { return pathToTorrentFile; }
            set
            {
                pathToTorrentFile = value;
                RaisePropertyChanged(() => PathToTorrentFile);
            }
        }

        Torrent torrent = null;

        public Torrent Torrent
        {
            get
            {
                return torrent;
            }
            private set
            {
                torrent = value;
                RaisePropertyChanged(() => Torrent);
            }
        }

        #region OpenFolderBrowserCommand

        private RelayCommand openFolderBrowserCommand;

        public ICommand OpenFolderBrowserCommand
        {
            get
            {
                if (openFolderBrowserCommand == null)
                    openFolderBrowserCommand = new RelayCommand(OpenFolderBrowserCommandExecute);

                return openFolderBrowserCommand;
            }
        }

        private void OpenFolderBrowserCommandExecute()
        {
            Messenger.Default.Send(new FolderBrowserShowDialogMessage((path, dialogResult) =>
            {
                if (dialogResult)
                    PathToFolder = path;
            }));

        }

        #endregion

        #region OkCommand

        private RelayCommand okCommand;

        public ICommand OkCommand
        {
            get
            {
                if (okCommand == null)
                    okCommand = new RelayCommand(OkCommandExecute, OkCommandCanExecute);

                return okCommand;
            }
        }

        private void OkCommandExecute()
        {
            Messenger.Default.Send("AddTorrentOkCommand");
        }

        private bool OkCommandCanExecute()
        {
            return Directory.Exists(PathToFolder) && !string.IsNullOrEmpty(PathToFolder);
        }

        #endregion

        #region CancelCommand

        private RelayCommand cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new RelayCommand(CancelCommandExecute);

                return cancelCommand;
            }
        }

        private void CancelCommandExecute()
        {
            Messenger.Default.Send("AddTorrentCancelCommand");
        }

        #endregion

        #region ViewLoadedCommand
        private RelayCommand viewLoadedComman;

        public ICommand ViewLoadedCommand
        {
            get
            {
                if (viewLoadedComman == null)
                    viewLoadedComman = new RelayCommand(ViewLoadedCommandExecute);
                return viewLoadedComman;
            }
        }

        private void ViewLoadedCommandExecute()
        {
            Torrent = MonoTorrent.Common.Torrent.Load(PathToTorrentFile);
        }
        #endregion
    }
}