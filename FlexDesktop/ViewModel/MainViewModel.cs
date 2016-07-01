using FlexDesktop.Messages;
using FlexDesktop.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MonoTorrent;
using MonoTorrent.BEncoding;
using MonoTorrent.Client;
using MonoTorrent.Client.Encryption;
using MonoTorrent.Common;
using MonoTorrent.Dht;
using MonoTorrent.Dht.Listeners;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FlexDesktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        SaveLoadTorrentManager saveLoadManager = new SaveLoadTorrentManager();

        ClientEngine engine;

        TorrentSettings tSettings = new TorrentSettings(50, 500);
        
        public MainViewModel()
        {
            tSettings.UseDht = true;

            EngineSettings eSettings = new EngineSettings();

            engine = new ClientEngine(eSettings);

            Torrents = new ObservableCollection<TorrentManagerWrapper>();

            foreach (TorrentManager manager in saveLoadManager.Load(tSettings))
            {
                engine.Register(manager);

                if(!manager.Complete)
                    manager.Start();

                TorrentManagerWrapper wrapper = new TorrentManagerWrapper(manager);

                Torrents.Add(wrapper);
            }
        }

        public void AddTorrent(string pathToTorrentFile)
        {
            Messenger.Default.Send(new AddTorrentShowDialog((dialogResult) =>
            {
                if (dialogResult == true)
                {
                    ViewModelLocator locator = new ViewModelLocator();
                    var addTorrentViewModel = locator.AddTorrent;
                    
                    TorrentManager manager = new TorrentManager(addTorrentViewModel.Torrent, addTorrentViewModel.PathToFolder, tSettings);
                    if (!engine.Contains(manager.InfoHash))
                    {
                        saveLoadManager.Add(manager);

                        engine.Register(manager);

                        manager.Start();

                        TorrentManagerWrapper wrapper = new TorrentManagerWrapper(manager);

                        Torrents.Add(wrapper);
                    }
                }
            }, pathToTorrentFile));
        }


        public ObservableCollection<TorrentManagerWrapper> Torrents
        {
            get;
            private set;
        }

        #region PauseTorrentCommand
        private RelayCommand pauseTorrentCommand;

        public ICommand PauseTorrentCommand
        {
            get
            {
                if (pauseTorrentCommand == null)
                    pauseTorrentCommand = new RelayCommand(PauseTorrentCommandExecute, PauseTorrentCommandCanExecute);

                return pauseTorrentCommand;
            }
        }

        private void PauseTorrentCommandExecute()
        {
            SelectedItem.Stop();
        }

        private bool PauseTorrentCommandCanExecute()
        {
            return SelectedItem != null && SelectedItem.State == TorrentState.Downloading;
        }
        #endregion

        #region StartTorrentCommand
        private RelayCommand startTorrentCommand;

        public ICommand StartTorrentCommand
        {
            get
            {
                if (startTorrentCommand == null)
                    startTorrentCommand = new RelayCommand(StartTorrentCommandExecute, StartTorrentCommandCanExecute);

                return startTorrentCommand;
            }
        }

        private void StartTorrentCommandExecute()
        {
            SelectedItem.Start();
        }

        private bool StartTorrentCommandCanExecute()
        {
            return SelectedItem != null && (SelectedItem.State == TorrentState.Paused || SelectedItem.State == TorrentState.Stopped || SelectedItem.State == TorrentState.Stopping);
        }
        #endregion

        #region OpenFolderTorrentCommand
        private RelayCommand<TorrentManagerWrapper> openFolderTorrentCommand;

        public ICommand OpenFolderTorrentCommand
        {
            get
            {
                if (openFolderTorrentCommand == null)
                    openFolderTorrentCommand = new RelayCommand<TorrentManagerWrapper>(OpenFolderTorrentCommandExecute, OpenFolderTorrentCommandCanExecute);

                return openFolderTorrentCommand;
            }
        }

        private bool OpenFolderTorrentCommandCanExecute(TorrentManagerWrapper torrent)
        {
            if (torrent == null)
                return false;

            return Directory.Exists(torrent.SavePath);
        }

        private void OpenFolderTorrentCommandExecute(TorrentManagerWrapper torrent)
        {
            Process.Start(torrent.SavePath);
        }
        #endregion

        #region RemoveTorrentCommand
        private RelayCommand removeTorrentCommand;

        public ICommand RemoveTorrentCommand
        {
            get
            {
                if (removeTorrentCommand == null)
                    removeTorrentCommand = new RelayCommand(RemoveTorrentCommandExecute, RemoveTorrentCommandCanExecute);

                return removeTorrentCommand;
            }
        }

        private void RemoveTorrentCommandExecute()
        {
            Messenger.Default.Send(new DeleteTorrentShowDialog((dialogResult) =>
            {
                if (dialogResult == true)
                {
                    SelectedItem.Stop();
                    engine.Unregister(SelectedItem.Manager);
                    saveLoadManager.Remove(SelectedItem.Manager);

                    ViewModelLocator locator = new ViewModelLocator();
                    var deleteTorrentVM = locator.DeleteTorrent;
                    
                    if(deleteTorrentVM.Delete == DeleteTorrent.DeleteWithDownloadedFiles)
                    {
                        foreach (var item in SelectedItem.Files)
                        {
                            File.Delete(SelectedItem.SavePath + "\\" + item.Path);
                        }

                        try
                        {
                            Directory.Delete(SelectedItem.SavePath, false);
                        }
                        catch (IOException)
                        {
                            
                        }
                        
                    }

                    
                    SelectedItem.Manager.Dispose();
                    Torrents.Remove(SelectedItem);
                }
            }));
            
        }

        private bool RemoveTorrentCommandCanExecute()
        {
            return SelectedItem != null;
        }
        #endregion

        private TorrentManagerWrapper selectedItem;

        public TorrentManagerWrapper SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        #region WindowClosingCommand
        private RelayCommand windowClosingCommand;

        public ICommand WindowClosingCommand
        {
            get
            {
                if (windowClosingCommand == null)
                    windowClosingCommand = new RelayCommand(WindowClosingCommandExecute);

                return windowClosingCommand;
            }
        }

        private void WindowClosingCommandExecute()
        {
            saveLoadManager.Save(engine.Torrents);   
        }
        #endregion


    }
}