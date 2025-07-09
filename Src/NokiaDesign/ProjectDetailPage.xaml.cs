/*using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

namespace NokiaDesign
{
    public sealed partial class ProjectDetailPage : Page
    {
        public ProjectDetailPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string nodeId)
            {
                var items = await ParserHelper.GetProjectDetailsAsync(nodeId);
                foreach (var item in items)
                {
                    var tb = new TextBlock { Text = item, TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap, Margin = new Windows.UI.Xaml.Thickness(0,0,0,20) };
                    ProjectContent.Children.Add(tb);
                }
            }
        }
    }
}*/
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

namespace NokiaDesign
{
    public sealed partial class ProjectDetailPage : Page
    {
        public ProjectDetailPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string nodeId)
            {
                var items = await ParserHelper.GetProjectDetailsAsync(nodeId);
                foreach (var item in items)
                {
                    var tb = new TextBlock { Text = item, TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap, Margin = new Windows.UI.Xaml.Thickness(0,0,0,20) };
                    ProjectContent.Children.Add(tb);
                }
            }
        }
    }
}

