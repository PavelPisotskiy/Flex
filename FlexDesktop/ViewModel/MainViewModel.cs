using FlexDesktop.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MonoTorrent;
using MonoTorrent.BEncoding;
using MonoTorrent.Client;
using MonoTorrent.Client.Encryption;
using MonoTorrent.Common;
using MonoTorrent.Dht;
using MonoTorrent.Dht.Listeners;
using System;
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
        ClientEngine engine;
        private DispatcherTimer dispatcherTimer;

        public MainViewModel()
        {
            EngineSettings eSettings = new EngineSettings();

            engine = new ClientEngine(eSettings);

            Torrents = new ObservableCollection<TorrentManager>();

            engine.Listener.Start();

            //IPEndPoint listenAddress = new IPEndPoint(IPAddress.Any, 34852);

            //// Create a listener which will process incoming/outgoing dht messages
            //DhtListener listener = new DhtListener(listenAddress);

            //// Create the dht engine
            //DhtEngine dht = new DhtEngine(listener);

            //// Connect the Dht engine to the MonoTorrent engine
            //engine.RegisterDht(dht);

            //// Start listening for dht messages and activate the DHT engine
            //listener.Start();

            //dht.Start();

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            dispatcherTimer.Start();

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //foreach (var item in engine.Torrents)
            //{
            //    Console.WriteLine(item.Peers.Leechs);
            //    Console.WriteLine(item.Peers.Seeds);
            //    Console.WriteLine(item.Peers.Available);

            //    foreach (var peer in item.GetPeers())
            //    {
            //        Console.WriteLine("\t" + peer.Uri);
            //        Console.WriteLine("\t" + peer.Monitor.DownloadSpeed);
            //        Console.WriteLine("\t" + peer.IsConnected);
            //    }
            //    Console.WriteLine(new string('-', 30));
            //}
        }

        public void AddTorrent(string pathToTorrentFile)
        {
            Messenger.Default.Send(new AddTorrentShowDialog((dialogResult) =>
            {
                if (dialogResult == true)
                {
                    ViewModelLocator locator = new ViewModelLocator();
                    var addTorrentViewModel = locator.AddTorrent;

                    TorrentSettings tSettings = new TorrentSettings(5, 100);
                    tSettings.UseDht = true;

                    TorrentManager manager = new TorrentManager(addTorrentViewModel.Torrent, addTorrentViewModel.PathToFolder, tSettings);
                    if (!engine.Contains(manager.InfoHash))
                    {
                        engine.Register(manager);

                        manager.Start();



                        Torrents.Add(manager);
                    }
                }
            }, pathToTorrentFile));
        }


        public ObservableCollection<TorrentManager> Torrents
        {
            get;
            private set;
        }

        #region PauseTorrentCommand
        private RelayCommand<TorrentManager> pauseTorrentCommand;

        public ICommand PauseTorrentCommand
        {
            get
            {
                if (pauseTorrentCommand == null)
                    pauseTorrentCommand = new RelayCommand<TorrentManager>(PauseTorrentCommandExecute, PauseTorrentCommandCanExecute);

                return pauseTorrentCommand;
            }
        }

        private void PauseTorrentCommandExecute(TorrentManager obj)
        {
            obj.Stop();
        }

        private bool PauseTorrentCommandCanExecute(TorrentManager arg)
        {
            return arg.State == TorrentState.Downloading;
        }
        #endregion

        #region StartTorrentCommand
        private RelayCommand<TorrentManager> startTorrentCommand;

        public ICommand StartTorrentCommand
        {
            get
            {
                if (startTorrentCommand == null)
                    startTorrentCommand = new RelayCommand<TorrentManager>(StartTorrentCommandExecute, StartTorrentCommandCanExecute);

                return startTorrentCommand;
            }
        }

        private void StartTorrentCommandExecute(TorrentManager obj)
        {
            obj.Start();
        }

        private bool StartTorrentCommandCanExecute(TorrentManager arg)
        {
            return arg.State == TorrentState.Paused || arg.State == TorrentState.Stopped || arg.State == TorrentState.Stopping;
        }
        #endregion

        #region OpenFolderTorrentCommand
        private RelayCommand<TorrentManager> openFolderTorrentCommand;

        public ICommand OpenFolderTorrentCommand
        {
            get
            {
                if (openFolderTorrentCommand == null)
                    openFolderTorrentCommand = new RelayCommand<TorrentManager>(OpenFolderTorrentCommandExecute);

                return openFolderTorrentCommand;
            }
        }

        private void OpenFolderTorrentCommandExecute(TorrentManager obj)
        {
            Process.Start(obj.SavePath);
        }
        #endregion

    }
}