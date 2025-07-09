using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NokiaDesign
{
    public sealed partial class RadialMenu : UserControl
    {
        private bool _expanded = true;

        public event Action<string> MenuItemClicked;

        public RadialMenu()
        {
            this.InitializeComponent();
            PositionButtons();
        }

        private void PositionButtons()
        {
            // Arrange buttons in a circle
            double centerX = 150, centerY = 150, radius = 100;
            double[] angles = { -90, 0, 90, 180 }; // degrees for 4 buttons
            Button[] buttons = { BtnInfo, BtnResearch, BtnNetwork, BtnChronology };
            for (int i = 0; i < buttons.Length; i++)
            {
                double angleRad = angles[i] * Math.PI / 180;
                double x = centerX + radius * Math.Cos(angleRad) - buttons[i].Width / 2;
                double y = centerY + radius * Math.Sin(angleRad) - buttons[i].Height / 2;
                buttons[i].SetValue(Canvas.LeftProperty, x);
                buttons[i].SetValue(Canvas.TopProperty, y);
            }
        }

        private void CenterButton_Click(object sender, RoutedEventArgs e)
        {
            _expanded = !_expanded;
            foreach (var btn in new[] { BtnInfo, BtnResearch, BtnNetwork, BtnChronology })
                btn.Visibility = _expanded ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Info_Click(object sender, RoutedEventArgs e) => MenuItemClicked?.Invoke("Information");
        private void Research_Click(object sender, RoutedEventArgs e) => MenuItemClicked?.Invoke("Research");
        private void Network_Click(object sender, RoutedEventArgs e) => MenuItemClicked?.Invoke("Network");
        private void Chronology_Click(object sender, RoutedEventArgs e) => MenuItemClicked?.Invoke("Chronology");
    }
}
