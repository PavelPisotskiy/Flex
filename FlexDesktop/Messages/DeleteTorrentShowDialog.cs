using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexDesktop.Messages
{
    class DeleteTorrentShowDialog
    {
        public Action<bool?> CallBack { get; private set; }

        public DeleteTorrentShowDialog(Action<bool?> callback)
        {
            CallBack = callback;
        }
    }
}
