using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NokiaDesign
{
    public sealed partial class ResearchPage2 : Page
    {
        public ResearchPage2()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var projects = await ParserHelper.GetProjectListAsync();
            ProjectsListView.ItemsSource = projects;
        }

        private void ProjectsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectsListView.SelectedItem is ProjectInfo project)
            {
                Frame.Navigate(typeof(ProjectDetailPage), project.NodeId);
            }
        }
    }
}
