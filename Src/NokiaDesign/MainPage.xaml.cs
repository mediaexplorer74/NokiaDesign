using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NokiaDesign
{
    public sealed partial class MainPage : Page
    {


        // Futuristic radial menu implementation
        /*public MainPage()
        {
            this.InitializeComponent();
            RadialMenuControl.MenuItemClicked += OnMenuItemClicked;
            ContentFrame.Navigate(typeof(InformationPage));
        }

        private void OnMenuItemClicked(string section)
        {
            switch (section)
            {
                case "Information":
                    ContentFrame.Navigate(typeof(InformationPage));
                    break;
                case "Research":
                    ContentFrame.Navigate(typeof(ResearchPage));
                    break;
                case "Network":
                    ContentFrame.Navigate(typeof(NetworkPage));
                    break;
                case "Chronology":
                    ContentFrame.Navigate(typeof(ChronologyPage));
                    break;
            }
        }
        */

        // Hamburger menu implementation
        public MainPage()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(InformationPage));
        }

        private void Information_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(InformationPage));
        }

        private void Projects_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ProjectsPage));
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
