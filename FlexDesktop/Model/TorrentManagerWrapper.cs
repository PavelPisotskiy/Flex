using MonoTorrent.Client;
using MonoTorrent.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FlexDesktop.Model
{
    public class TorrentManagerWrapper : DependencyObject
    {
        private readonly DispatcherTimer dispatcherTimer;
        private readonly TorrentManager manager;

        public TorrentManagerWrapper(TorrentManager manager)
        {
            this.manager = manager;

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Progress = manager.Progress;
            DownloadSpeed = manager.Monitor.DownloadSpeed;
            UploadSpeed = manager.Monitor.UploadSpeed;
        }
        
        public TorrentManager Manager { get { return manager; } }

        public string Name { get { return manager.Torrent.Name; } }

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            private set { SetValue(ProgressProperty, value); }
        }
        
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(double), typeof(TorrentManagerWrapper), new PropertyMetadata(0.0));


        public int DownloadSpeed
        {
            get { return (int)GetValue(DownloadSpeedProperty); }
            private set { SetValue(DownloadSpeedProperty, value); }
        }
        
        public static readonly DependencyProperty DownloadSpeedProperty =
            DependencyProperty.Register("DownloadSpeed", typeof(int), typeof(TorrentManagerWrapper), new PropertyMetadata(0));

        public int UploadSpeed
        {
            get { return (int)GetValue(UploadSpeedProperty); }
            private set { SetValue(UploadSpeedProperty, value); }
        }
        
        public static readonly DependencyProperty UploadSpeedProperty =
            DependencyProperty.Register("UploadSpeed", typeof(int), typeof(TorrentManagerWrapper), new PropertyMetadata(0));

        public string SavePath { get { return manager.SavePath; } }

        public TorrentState State { get { return manager.State; } }

        public TorrentFile[] Files { get { return manager.Torrent.Files; } }

        public string PathToTorrentFile { get { return manager.Torrent.TorrentPath; } }

        public string FileNameWithExtension { get { return Path.GetFileName(manager.Torrent.TorrentPath); } }
        
        public void Start()
        {
            manager.Start();
        }

        public void Pause()
        {
            manager.Pause();
        }

        public void Stop()
        {
            manager.Stop();
        }



    }
}
