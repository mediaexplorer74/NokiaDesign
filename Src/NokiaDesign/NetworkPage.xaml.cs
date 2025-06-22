using System;
using System.Net.Http;
using Windows.UI.Xaml.Controls;
using HtmlAgilityPack;
using Windows.UI.Xaml.Navigation;

namespace NokiaDesign
{
    public sealed partial class NetworkPage : Page
    {
        public NetworkPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string url = "https://nokiadesignarchive.aalto.fi/index.html";
            try
            {
                HttpClient client = new HttpClient();
                string html = await client.GetStringAsync(new Uri(url));
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var nodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'news-item')]");
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        var tb = new TextBlock
                        {
                            Text = node.InnerText.Trim(),
                            TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap,
                            Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, 20)
                        };
                        NetworkContent.Children.Add(tb);
                    }
                }
                else
                {
                    NetworkContent.Children.Add(new TextBlock
                    {
                        Text = "No network items found.",
                        Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, 20)
                    });
                }
            }
            catch (Exception ex)
            {
                NetworkContent.Children.Add(new TextBlock
                {
                    Text = $"Error loading network: {ex.Message}",
                    Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, 20)
                });
            }
        }
    }
}

