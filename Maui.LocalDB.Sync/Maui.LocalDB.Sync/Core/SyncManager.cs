using Maui.LocalDB.Sync.TTL;

namespace Maui.LocalDB.Sync.Core
{

    public interface ISyncMetadataProvider<TKey>
    {
        Task<DateTime?> GetLastSyncedUtcAsync(TKey scope);
        Task SetLastSyncedUtcAsync(TKey scope, DateTime timestamp);
    }

    public class SyncManager<T, TKey> where T : ISyncableEntity<TKey> where TKey : notnull
    {
        private readonly Func<Task<IEnumerable<T>>> fetchRemote;
        private readonly Func<Task<IEnumerable<T>>> fetchLocal;
        private readonly Func<IEnumerable<T>, Task> saveLocal;
        private readonly Func<T, T, T> resolveConflict;
        private readonly ISyncMetadataProvider<string>? metadataProvider;
        private readonly string syncScope;
        private readonly ITtlPolicy<T>? ttlPolicy;

        public SyncManager(
            Func<Task<IEnumerable<T>>> fetchRemote,
            Func<Task<IEnumerable<T>>> fetchLocal,
            Func<IEnumerable<T>, Task> saveLocal,
            Func<T, T, T> resolveConflict,
            ISyncMetadataProvider<string>? metadataProvider = null,
            string syncScope = "default",
            ITtlPolicy<T>? ttlPolicy = null)
        {
            this.fetchRemote = fetchRemote;
            this.fetchLocal = fetchLocal;
            this.saveLocal = saveLocal;
            this.resolveConflict = resolveConflict;
            this.metadataProvider = metadataProvider;
            this.syncScope = syncScope;
            this.ttlPolicy = ttlPolicy;
        }

        public async Task<List<SyncResult<T>>> SyncAsync()
        {
            IEnumerable<T> remoteList;
            try
            {
                remoteList = await RetryHelper.ExecuteWithRetry(fetchRemote);
            }
            catch (Exception ex)
            {
                return new List<SyncResult<T>>
                {
                    new(default!, SyncStatus.Conflict, ex)
                };
            }

            var remoteItems = remoteList.ToDictionary(i => i.Id);
            var localItems = (await fetchLocal()).ToDictionary(i => i.Id);

            var results = new List<SyncResult<T>>();
            var finalItems = new List<T>();

            foreach (var remote in remoteItems)
            {
                if (localItems.TryGetValue(remote.Key, out var local))
                {
                    if (remote.Value.LastModified > local.LastModified)
                    {
                        finalItems.Add(remote.Value);
                        results.Add(new SyncResult<T>(remote.Value, SyncStatus.Updated));
                    }
                    else if (local.LastModified > remote.Value.LastModified)
                    {
                        var resolved = resolveConflict(local, remote.Value);
                        finalItems.Add(resolved);
                        results.Add(new SyncResult<T>(resolved, SyncStatus.Conflict));
                    }
                    else
                    {
                        finalItems.Add(local);
                        results.Add(new SyncResult<T>(local, SyncStatus.Synced));
                    }
                }
                else
                {
                    finalItems.Add(remote.Value);
                    results.Add(new SyncResult<T>(remote.Value, SyncStatus.Inserted));
                }
            }

            foreach (var local in localItems)
            {
                if (!remoteItems.ContainsKey(local.Key))
                {
                    results.Add(new SyncResult<T>(local.Value, SyncStatus.Deleted));
                }
            }

            if (ttlPolicy != null)
            {
                var expiredItems = finalItems.Where(ttlPolicy.ShouldDelete).ToList();
                foreach (var expired in expiredItems)
                {
                    results.Add(new SyncResult<T>(expired, SyncStatus.Deleted));
                }

                finalItems = finalItems.Except(expiredItems).ToList();
            }

            await RetryHelper.ExecuteWithRetry(() => saveLocal(finalItems));

            if (metadataProvider != null)
            {
                await metadataProvider.SetLastSyncedUtcAsync(syncScope, DateTime.UtcNow);
            }

            return results;
        }
    }

}
