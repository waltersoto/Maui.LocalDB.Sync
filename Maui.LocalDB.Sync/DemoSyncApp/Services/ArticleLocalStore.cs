using DemoSyncApp.Models;
using Maui.LocalDB.Sync.Storage;

namespace DemoSyncApp.Services
{
    public class ArticleLocalStore
    {
        private readonly SQLiteStore<Article, Guid> _store;

        public ArticleLocalStore()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "articles.db");
            _store = new SQLiteStore<Article, Guid>(dbPath);
        }

        public Task<List<Article>> GetAllAsync() => _store.GetAllAsync();
        public Task SaveAsync(IEnumerable<Article> articles) => _store.SaveAsync(articles);

        public Task DeleteAllAsync() => _store.DeleteAsync(_ => true);

    }
}
