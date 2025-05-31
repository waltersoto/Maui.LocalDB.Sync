using DemoSyncApp.Models;
using DemoSyncApp.Services;

namespace DemoSyncApp
{
    public partial class MainPage : ContentPage
    {
        private readonly SyncService _syncService = new();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var existingArticles = await _syncService.GetLocalArticlesAsync();
            ArticlesList.ItemsSource = existingArticles;
        }


        private async void OnClearClicked(object sender, EventArgs e)
        {
            await _syncService.ClearLocalArticlesAsync();
            ArticlesList.ItemsSource = null;
        }


        private async void OnSyncClicked(object sender, EventArgs e)
        {

            var syncResults = await _syncService.SyncAsync();
            var articles = syncResults.Select(r => r.Entity).ToList();
            ArticlesList.ItemsSource = articles;

            await DisplayAlert("Sync Complete", $"{articles.Count} articles stored locally.", "OK");

        }
    }

}
