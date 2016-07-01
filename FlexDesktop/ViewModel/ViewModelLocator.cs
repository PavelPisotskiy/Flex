/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:FlexDesktop.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace FlexDesktop.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddTorrentWindowViewModel>();
            SimpleIoc.Default.Register<DeleteTorrentWindowViewModel>();
        }
        
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public AddTorrentWindowViewModel AddTorrent
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddTorrentWindowViewModel>();
            }
        }

        public DeleteTorrentWindowViewModel DeleteTorrent
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DeleteTorrentWindowViewModel>();
            }
        }

        public static void Cleanup()
        {

        }
    }
}