using MonoTorrent.Client;
using MonoTorrent.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            //Progress = manager.Progress;
            double progress = 0;
            long totalSize = 0;
            long bytesDownloaded = 0;

            int timeLeft = 0;

            foreach (var file in manager.Torrent.Files)
            {
                if(file.Priority != Priority.DoNotDownload)
                {
                    totalSize += file.Length;
                    bytesDownloaded += file.BytesDownloaded;
                }
            }

            try
            {
                progress = (bytesDownloaded * 100.0) / TotalSize;
            }
            catch (DivideByZeroException)
            {
                progress = 0;
            }

            try
            {
                timeLeft = Convert.ToInt32((totalSize - bytesDownloaded) / manager.Monitor.DownloadSpeed);
                TimeLeft = new TimeSpan(0, 0, timeLeft);
            }
            catch (DivideByZeroException)
            {
                timeLeft = 0;
                TimeLeft = new TimeSpan(0, 0, timeLeft);
            }
            catch(OverflowException)
            {
                TimeLeft = Timeout.InfiniteTimeSpan;
            }

            
            
            Progress = progress;
            TotalSize = totalSize;
            BytesDownloaded = bytesDownloaded;

            Console.WriteLine("Progress: {0}", Progress);
            Console.WriteLine("Total Sizes: {0}", TotalSize);
            Console.WriteLine("Bytes Downloaded: {0}", BytesDownloaded);
            Console.WriteLine(new string('-',50));

            DownloadSpeed = manager.Monitor.DownloadSpeed;
            UploadSpeed = manager.Monitor.UploadSpeed;
        }

        public TimeSpan TimeLeft
        {
            get { return (TimeSpan)GetValue(TimeLeftProperty); }
            set { SetValue(TimeLeftProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeLeft.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeLeftProperty =
            DependencyProperty.Register("TimeLeft", typeof(TimeSpan), typeof(TorrentManagerWrapper), new PropertyMetadata(new TimeSpan()));



        public long BytesDownloaded
        {
            get { return (long)GetValue(BytesDownloadedProperty); }
            set { SetValue(BytesDownloadedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BytesDownloaded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BytesDownloadedProperty =
            DependencyProperty.Register("BytesDownloaded", typeof(long), typeof(TorrentManagerWrapper));



        public long TotalSize
        {
            get { return (long)GetValue(TotalSizeProperty); }
            set { SetValue(TotalSizeProperty, value); }
        }
        
        public static readonly DependencyProperty TotalSizeProperty =
            DependencyProperty.Register("TotalSize", typeof(long), typeof(TorrentManagerWrapper));



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
