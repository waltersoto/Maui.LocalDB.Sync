using DemoSyncApp.Models;
using Maui.LocalDB.Sync.Conflicts;
using Maui.LocalDB.Sync.Core;

namespace DemoSyncApp.Services
{
    public class SyncService
    {
        private readonly ArticleLocalStore _localStore = new();
        private readonly DummyRemoteService _remoteService = new();

        public List<SyncResult<Article>> LastSyncResults { get; private set; }

        public async Task<List<SyncResult<Article>>> SyncAsync()
        {
            var manager = new SyncManager<Article, Guid>(
                fetchRemote: _remoteService.GetArticlesAsync,
                fetchLocal: async () => await _localStore.GetAllAsync(),
                saveLocal: _localStore.SaveAsync,
                resolveConflict: new PreferRemoteStrategy<Article>().Resolve
            );

            LastSyncResults = await manager.SyncAsync();
            return LastSyncResults;
        }

        public Task ClearLocalArticlesAsync() => _localStore.DeleteAllAsync();

        public Task<List<Article>> GetLocalArticlesAsync() => _localStore.GetAllAsync();
    }
}
