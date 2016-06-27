using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexDesktop.Messages
{
    class FolderBrowserShowDialogMessage
    {
        public Action<string, bool> CallBack { get; private set; }

        public FolderBrowserShowDialogMessage(Action<string, bool> callback)
        {
            CallBack = callback;
        }
    }
}
