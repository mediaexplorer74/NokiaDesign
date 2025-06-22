using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NokiaDesign
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(InformationPage));
        }

        private void Information_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(InformationPage));
        }

        private void Research_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ResearchPage));
        }

        private void Network_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(NetworkPage));
        }

        private void Chronology_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ChronologyPage));
        }
    }
}
