using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;

namespace FlexDesktop.Helpers
{
    class SingleInstanceApplicationWrapper : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {
        private App application;

        public SingleInstanceApplicationWrapper()
        {
            this.IsSingleInstance = true;
        }

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            application = new App();
            application.InitializeComponent();
            application.Run();

            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            application.OnStartupNextInstance(eventArgs.CommandLine.ToArray());
        }
    }
}
