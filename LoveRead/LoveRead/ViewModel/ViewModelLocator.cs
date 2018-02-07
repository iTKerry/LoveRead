using System.Text;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using LoveRead.Infrastructure;
using LoveRead.Infrastructure.Services;
using LoveRead.Views.Main;
using LoveRead.Views.Tabs.BookDetails;
using LoveRead.Views.Tabs.ReadBook;
using Microsoft.Practices.ServiceLocation;
using ScrapySharp.Network;

namespace LoveRead.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            DispatcherHelper.Initialize();

            var browser = new ScrapingBrowser{Encoding = Encoding.GetEncoding("windows-1251")};

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ReadBookViewModel>();
            SimpleIoc.Default.Register<BookDetailsViewModel>();

            SimpleIoc.Default.Register<IDownloadService, DownloadService>();
            SimpleIoc.Default.Register<IDocService, DocService>();
            SimpleIoc.Default.Register<IMessangerService, MessangerService>();
            SimpleIoc.Default.Register<ILibraryScrapper>(() => new LibraryScrapper(browser, SimpleIoc.Default.GetInstance<IMessangerService>()));
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public ReadBookViewModel ReadBook => ServiceLocator.Current.GetInstance<ReadBookViewModel>();
        public BookDetailsViewModel BookDetails => ServiceLocator.Current.GetInstance<BookDetailsViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}