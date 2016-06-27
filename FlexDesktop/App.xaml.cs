using System.Windows;
using GalaSoft.MvvmLight.Threading;
using FlexDesktop.View;

namespace FlexDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new View.MainWindow();
            mainWindow.Show();

            if (e.Args.Length > 0)
            {
                ViewModel.ViewModelLocator locator = new ViewModel.ViewModelLocator();
                locator.Main.AddTorrent(e.Args[0]);
            }
        }

        public void OnStartupNextInstance(string[] args)
        {
            App.Current.MainWindow.Activate();

            if(args.Length > 0)
            {
                ViewModel.ViewModelLocator locator = new ViewModel.ViewModelLocator();
                locator.Main.AddTorrent(args[0]);
            }
        }

    }
}
