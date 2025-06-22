using System;
using System.Net.Http;
using Windows.UI.Xaml.Controls;
using HtmlAgilityPack;
using Windows.UI.Xaml.Navigation;

namespace NokiaDesign
{
    public sealed partial class ResearchPage : Page
    {
        public ResearchPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string url = "https://nokiadesignarchive.aalto.fi/research.html";
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
                        var tb = new TextBlock { Text = node.InnerText.Trim(), TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap, Margin = new Windows.UI.Xaml.Thickness(0,0,0,20) };
                        ResearchContent.Children.Add(tb);
                    }
                }
                else
                {
                    ResearchContent.Children.Add(new TextBlock { Text = "No research items found.", Margin = new Windows.UI.Xaml.Thickness(0,0,0,20) });
                }
            }
            catch (Exception ex)
            {
                ResearchContent.Children.Add(new TextBlock { Text = $"Error loading research: {ex.Message}", Margin = new Windows.UI.Xaml.Thickness(0,0,0,20) });
            }
        }
    }
}
