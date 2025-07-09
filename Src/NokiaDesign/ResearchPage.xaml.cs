using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.UI.Xaml;
using System.Threading.Tasks;

namespace NokiaDesign
{
    public sealed partial class ResearchPage : Page
    {
        // Префиксы и лимиты (можно расширить)
        private string[] prefixes = { "A", "B", "C" };
        private int[] maxNumbers = { 755, 50, 50 }; // можно скорректировать после анализа
        private int curPrefix = 0;
        private int curNumber = 1;
        // User-Agent для Android Chrome (можно менять)
        private string mobileUserAgent = "Mozilla/5.0 (Linux; Android 10; SM-G973F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Mobile Safari/537.36";

        public ResearchPage()
        {
            this.InitializeComponent();
            this.Loaded += ResearchPage_Loaded;
            MobileWebView.NavigationCompleted += MobileWebView_NavigationCompleted;
        }

        private void ResearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            curPrefix = 0;
            curNumber = 1;
            LoadProject();
        }

        private void BtnFirst_Click(object sender, RoutedEventArgs e)
        {
            curNumber = 1;
            LoadProject();
        }
        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (curNumber > 1) curNumber--;
            else if (curPrefix > 0) { curPrefix--; curNumber = maxNumbers[curPrefix]; }
            LoadProject();
        }
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (curNumber < maxNumbers[curPrefix]) curNumber++;
            else if (curPrefix < prefixes.Length - 1) { curPrefix++; curNumber = 1; }
            LoadProject();
        }
        private void BtnLast_Click(object sender, RoutedEventArgs e)
        {
            curNumber = maxNumbers[curPrefix];
            LoadProject();
        }

        private void LoadProject()
        {
            string nodeId = $"{prefixes[curPrefix]}{curNumber.ToString("D4")}";
            var url = $"https://nokiadesignarchive.aalto.fi/research.html";
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
            request.Headers.Append("User-Agent", mobileUserAgent);
            MobileWebView.NavigateWithHttpRequestMessage(request);
            //TxtProjectIndex.Text = $"{prefixes[curPrefix]} {curNumber} / {maxNumbers[curPrefix]}";
        }

        private async void MobileWebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            // CSS для скрытия панели навигации сайта
            string css = "body .nav-btn, body .nav, body .back-btn, body .project-nav, body .project-controls { display:none !important; }";
            // Инъекция meta viewport
            string metaViewport = @"if(!document.querySelector('meta[name=viewport]')) "+
                "{ var m=document.createElement('meta'); m.name='viewport'; m.content='width=850, initial-scale=1'; document.head.appendChild(m); } "+
                " else { document.querySelector('meta[name=viewport]').setAttribute('content', 'width=850, initial-scale=1'); }";
            string js = $@"
                var style = document.createElement('style');
                style.innerHTML = `{css}`;
                document.head.appendChild(style);
                {metaViewport}
            ";
            try { await MobileWebView.InvokeScriptAsync("eval", new[] { js }); } catch { }
        }
    }
}
