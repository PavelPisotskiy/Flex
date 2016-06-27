using FlexDesktop.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MonoTorrent.BEncoding;
using MonoTorrent.Client;
using MonoTorrent.Client.Encryption;
using MonoTorrent.Common;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace FlexDesktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        ClientEngine engine;

        public MainViewModel()
        {
            EngineSettings eSettings = new EngineSettings();
            engine = new ClientEngine(eSettings);

            Torrents = new ObservableCollection<TorrentManager>();

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            RaisePropertyChanged("Torrents");
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
                    if(!engine.Contains(manager.InfoHash))
                    {
                        engine.Register(manager);

                        manager.Start();
                        engine.Listener.Start();

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
    }
}