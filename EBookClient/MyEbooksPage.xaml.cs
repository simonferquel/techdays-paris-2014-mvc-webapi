using System.Collections.ObjectModel;
using Windows.UI.Core;
using EBookClient.Common;
using EBookClient.Queries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace EBookClient
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MyEbooksPage : Page
    {


        public class EbookViewModel
        {
            EbookResume _item;
            public EbookViewModel(EbookResume item, BitmapImage thumb)
            {
                _item = item;
                Thumbnail = thumb;
            }

            public string Id { get { return _item.Id; } }
            public string Summary { get { return _item.Summary; } }
            public string Title { get { return _item.Title; } }
            public int PartCount { get { return _item.PartCount; } }
            public BitmapImage Thumbnail { get; set; }
        }
        private async Task LoadMyEbooksAsync()
        {
            List<EbookViewModel> viewModels = new List<EbookViewModel>();
            var ebooks = await new GetMyEbooksQuery().ExecuteAsync(OAuthSettings.AccessToken);
            foreach (var ebook in ebooks)
            {
                var img = new BitmapImage();
                var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream();
                stream.GetOutputStreamAt(0).AsStreamForWrite().Write(ebook.Thumbnail, 0, ebook.Thumbnail.Length);
                await img.SetSourceAsync(stream);
                viewModels.Add(new EbookViewModel(ebook, img));
            }
            flipMyBooks.ItemsSource = viewModels;

        }


        private async void MyBooksSelectionChanged(object sender, RoutedEventArgs e)
        {
            var selected = flipMyBooks.SelectedItem as EbookViewModel;
            if (selected == null)
            {
                return;
            }

            ProgressRingBooks.IsActive = true;

            ObservableCollection<ImageSource> pdf = null;

            if (gvImages.ItemsSource != null)
            {
                pdf = gvImages.ItemsSource as ObservableCollection<ImageSource>;
                gvImages.SelectedIndex = 0;
                pdf.Clear();
            }
            else
            {
                pdf = new ObservableCollection<ImageSource>();
                gvImages.ItemsSource = pdf;
            }

            var query = new GetAllEbookPartsQuery();
            await query.ExecuteAsync(OAuthSettings.AccessToken, selected.Id, selected.PartCount,
                async (sourceStream) =>
                {
                    var stream = new InMemoryRandomAccessStream();
                    await sourceStream.CopyToAsync(stream.AsStreamForWrite());
                    var doc = await PdfDocument.LoadFromStreamAsync(stream);
                    for (uint i = 0; i < doc.PageCount; ++i)
                    {
                        var bmpStream = new Windows.Storage.Streams.InMemoryRandomAccessStream();

                        await doc.GetPage(i).RenderToStreamAsync(bmpStream);

                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            var bmp = new BitmapImage();
                            bmp.SetSource(bmpStream);
                            pdf.Add(bmp);
                        });
                    }
                });

            ProgressRingBooks.IsActive = false;
        }


        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public MyEbooksPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            LoadMyEbooksAsync();
        }



        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

    }
}
