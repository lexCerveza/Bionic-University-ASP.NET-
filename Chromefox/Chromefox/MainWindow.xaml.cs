using System.Windows;

namespace Chromefox
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendRequest_OnClick(object sender, RoutedEventArgs e)
        {
            var validatedUrl = UrlValidator.ValidateUrl(SearchBox.Text);
            if (string.IsNullOrEmpty(validatedUrl))
            {
                MessageBox.Show("Invalid URL", "Error");
                return;
            }

            using (var httpClient = new HttpClient(validatedUrl, 80))
            {
                ContentBlock.Text = httpClient.HttpGetRequest();
            }
        }
    }
}
