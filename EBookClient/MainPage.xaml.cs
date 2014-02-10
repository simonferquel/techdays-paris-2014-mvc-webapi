using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EBookClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UpdateUserLayoutAsync();
            base.OnNavigatedTo(e);
        }

        private async Task UpdateUserLayoutAsync()
        {
            if (OAuthSettings.AccessToken == null)
            {
                BtnLogin.Visibility = Visibility.Visible;
                BtnLogout.Visibility = Visibility.Collapsed;
                BtnWebView.Visibility = Visibility.Collapsed;
                BtnMyEbooks.Visibility = Visibility.Collapsed;
                txtUsername.Visibility = Visibility.Collapsed;
            }
            else
            {
                var userInfo = await new Queries.WhoAmiQuery().ExecuteAsync(OAuthSettings.AccessToken);
                txtUsername.Text = string.Format("Bienvenue {0} !", userInfo.UserName);

                BtnLogin.Visibility = Visibility.Collapsed;
                BtnLogout.Visibility = Visibility.Visible;
                BtnWebView.Visibility = Visibility.Visible;
                BtnMyEbooks.Visibility = Visibility.Visible;
                txtUsername.Visibility = Visibility.Visible;
            }
        }

        private async void OnLoginClick(object sender, RoutedEventArgs e)
        {
            await EnsureLoggedInAsync();

            

            UpdateUserLayoutAsync();
        }

        private async Task EnsureLoggedInAsync()
        {
            var accessToken = await new Queries.LoginPassivelyQuery().ExecuteAsync();
            if (accessToken != null)
            {
                return;
            }

            var webView = new WebView() { Margin = new Thickness(50) };
            Grid.SetRow(webView, 1);
            layoutRoot.Children.Add(webView);
            accessToken = await new Queries.LoginInteractivelyAsync().ExecuteAsync(webView, layoutRoot);
        }

      

        private void OnLogoutClick(object sender, RoutedEventArgs e)
        {
            OAuthSettings.AccessToken = null;
            OAuthSettings.RefreshToken = null;

            UpdateUserLayoutAsync();
        }

        private async void OnWebviewClick(object sender, RoutedEventArgs e)
        {
            await EnsureLoggedInAsync();

            var webView = new WebView() { Margin = new Thickness(50) };

            webView.Loaded += (s, a) =>
            {
                var message = new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost:20394/demowebviewauth"));
                //message.Headers.Authorization = new HttpCredentialsHeaderValue("Bearer", OAuthSettings.AccessToken);
                webView.NavigateWithHttpRequestMessage(message);
            };

            Grid.SetRow(webView, 1);

            layoutRoot.Children.Add(webView);
            await Task.Delay(15000);
            layoutRoot.Children.Remove(webView);
        }

        private async void OnMyEBooksClick(object sender, RoutedEventArgs e)
        {
            await EnsureLoggedInAsync();
            this.Frame.Navigate(typeof(MyEbooksPage));

        }
    }
}
