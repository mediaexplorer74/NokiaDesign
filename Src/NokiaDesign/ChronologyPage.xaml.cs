using System;
using System.Net.Http;
using Windows.UI.Xaml.Controls;
using HtmlAgilityPack;
using Windows.UI.Xaml.Navigation;

namespace NokiaDesign
{
    public sealed partial class ChronologyPage : Page
    {
        public ChronologyPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string url = "https://nokiadesignarchive.aalto.fi/timeline.html";
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
                        var tb = new TextBlock { 
                            Text = node.InnerText.Trim(), TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap, 
                            Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, 20) };
                        TimelineContent.Children.Add(tb);
                    }
                }
                else
                {
                    TimelineContent.Children.Add(new TextBlock { 
                        Text = "No timeline items found.", 
                        Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, 20) });
                }
            }
            catch (Exception ex)
            {
                TimelineContent.Children.Add(new TextBlock { Text = $"Error loading timeline: {ex.Message}", 
                    Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, 20) });
            }
        }
    }
}
