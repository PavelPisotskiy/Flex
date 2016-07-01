using MonoTorrent.BEncoding;
using MonoTorrent.Client;
using MonoTorrent.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FlexDesktop.Model
{
    class SaveLoadTorrentManager
    {
        string pathToFolderApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Flex";

        
        List<ResumeTorrentInfo> torrents = new List<ResumeTorrentInfo>();

        public SaveLoadTorrentManager()
        {
            if (!Directory.Exists(pathToFolderApplicationData + "\\Torrents\\"))
                Directory.CreateDirectory(pathToFolderApplicationData + "\\Torrents\\");

        }

        public void Add(TorrentManager manager)
        {
            File.Copy(manager.Torrent.TorrentPath, pathToFolderApplicationData + "\\Torrents\\" + Path.GetFileName(manager.Torrent.TorrentPath), true);

            ResumeTorrentInfo r = new ResumeTorrentInfo() { FileName = Path.GetFileName(manager.Torrent.TorrentPath), SavePath = Path.GetDirectoryName(manager.SavePath) };

            torrents.Add(r);
        }

        public void Remove(TorrentManager manager)
        {
            int index = -1;

            for (int i = 0; i < torrents.Count; i++)
            {
                if (torrents[i].FileName.Equals(Path.GetFileName(manager.Torrent.TorrentPath)) && torrents[i].SavePath.Equals(Path.GetDirectoryName(manager.SavePath)))
                {
                    index = i;
                    torrents.RemoveAt(index);
                    File.Delete(pathToFolderApplicationData + "\\Torrents\\" + Path.GetFileName(manager.Torrent.TorrentPath));
                    break;
                }
            }
            
        }

        public void Save(IEnumerable<TorrentManager> managers)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(pathToFolderApplicationData + "\\Torrents.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, torrents);
            }

            SaveFastResume(managers.ToArray());
        }

        public IEnumerable<TorrentManager> Load(TorrentSettings settings)
        {
            List<TorrentManager> managers = new List<TorrentManager>();
            
            if(File.Exists(pathToFolderApplicationData + "\\Torrents.dat"))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream fs = new FileStream(pathToFolderApplicationData + "\\Torrents.dat", FileMode.OpenOrCreate))
                {
                    torrents = (List<ResumeTorrentInfo>)formatter.Deserialize(fs);
                }

                foreach (var torr in torrents)
                {
                    string pathWithName = pathToFolderApplicationData + "\\Torrents\\" + torr.FileName;
                    if (File.Exists(pathWithName))
                    {
                        Torrent torrent = Torrent.Load(pathWithName);
                        TorrentManager manager = new TorrentManager(torrent, torr.SavePath, settings);
                        managers.Add(manager);
                    }
                }

                LoadFastResume(managers);
            }
            
            return managers;
        }

        private void SaveFastResume(TorrentManager[] managers)
        {
            BEncodedList list = new BEncodedList();

            foreach (TorrentManager manager in managers)
            {
                FastResume data = manager.SaveFastResume();
                BEncodedDictionary fastResume = data.Encode();
                list.Add(fastResume);
            }

            File.WriteAllBytes(pathToFolderApplicationData + "\\FastResume.dat", list.Encode());
        }

        private void LoadFastResume(List<TorrentManager> managers)
        {
            BEncodedList list = (BEncodedList)BEncodedValue.Decode(File.ReadAllBytes(pathToFolderApplicationData + "\\FastResume.dat"));
            foreach (BEncodedDictionary fastResume in list)
            {
                FastResume data = new FastResume(fastResume);
                foreach (TorrentManager manager in managers)
                    if (manager.InfoHash == data.Infohash)
                        manager.LoadFastResume(data);

            }
        }
    }
}
