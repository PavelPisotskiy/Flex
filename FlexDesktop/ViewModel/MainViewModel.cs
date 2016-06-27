using FlexDesktop.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MonoTorrent.BEncoding;
using MonoTorrent.Client;
using MonoTorrent.Client.Encryption;
using MonoTorrent.Common;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace FlexDesktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private DispatcherTimer dispatcherTimer;

        ClientEngine engine;

        public MainViewModel()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);


            EngineSettings eSettings = new EngineSettings();
           

            engine = new ClientEngine(eSettings);

           
            

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DownloadSpeed = Math.Round(manager.Monitor.DownloadSpeed / 1024.0, 2);
            Downloaded = manager.Monitor.DataBytesDownloaded;
            Console.WriteLine(manager.State);
            Console.WriteLine(manager.Progress);
            Console.WriteLine(manager.Peers.Seeds);
            Console.WriteLine(manager.Error);
            Console.WriteLine(new string('-', 30));
        }

        private double downloadSpeed;

        public double DownloadSpeed
        {
            get { return downloadSpeed; }
            set
            {
                downloadSpeed = value;
                RaisePropertyChanged(() => DownloadSpeed);
            }
        }

        private long downloaded;

        public long Downloaded
        {
            get { return downloaded; }
            set
            {
                downloaded = value;
                RaisePropertyChanged(() => Downloaded);
            }
        }



        TorrentManager manager;

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

                    manager = new TorrentManager(addTorrentViewModel.Torrent, addTorrentViewModel.PathToFolder, tSettings);

                    
                    engine.Register(manager);
                    
                    manager.Start();
                    engine.Listener.Start();


                    dispatcherTimer.Start();
                }
            }, pathToTorrentFile));
        }

    }
}