using FlexDesktop.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Input;

namespace FlexDesktop.ViewModel
{
    public class DeleteTorrentWindowViewModel : ViewModelBase
    {
        public DeleteTorrentWindowViewModel()
        {
        }

        private DeleteTorrent delete = DeleteTorrent.DeleteFromList;

        public DeleteTorrent Delete
        {
            get { return delete; }
            set
            {
                delete = value;
                RaisePropertyChanged(() => Delete);
            }
        }

        #region OkCommand

        private RelayCommand okCommand;

        public ICommand OkCommand
        {
            get
            {
                if (okCommand == null)
                    okCommand = new RelayCommand(OkCommandExecute);

                return okCommand;
            }
        }

        private void OkCommandExecute()
        {
            Messenger.Default.Send("DeleteTorrentOkCommand");
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
            Messenger.Default.Send("DeleteTorrentCancelCommand");
        }

        #endregion


    }
}