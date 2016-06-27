using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexDesktop.Messages
{
    class AddTorrentShowDialog
    {
        public Action<bool?> CallBack { get; private set; }

        public string PathToTorrentFile { get; private set; }

        public AddTorrentShowDialog(Action<bool?> callback, string pathToTorrentFile)
        {
            CallBack = callback;
            PathToTorrentFile = pathToTorrentFile;
        }
    }
}
