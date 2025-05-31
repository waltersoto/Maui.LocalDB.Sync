using Maui.LocalDB.Sync.Core;
using SQLite;

namespace Maui.LocalDB.Sync.Storage
{
    public class SQLiteStore<T, TKey> where T : class, ISyncableEntity<TKey>, new() where TKey : notnull
    {
        private readonly SQLiteAsyncConnection connection;

        public SQLiteStore(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            connection.CreateTableAsync<T>().Wait();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await connection.Table<T>().ToListAsync();
        }

        public async Task SaveAsync(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                var existing = await connection.FindAsync<T>(item.Id);
                if (existing != null)
                {
                    await connection.UpdateAsync(item);
                }
                else
                {
                    await connection.InsertAsync(item);
                }
            }
        }

        public async Task DeleteAsync(Func<T, bool> predicate)
        {
            var allItems = await GetAllAsync();
            var itemsToDelete = allItems.Where(predicate).ToList();
            foreach (var item in itemsToDelete)
            {
                await connection.DeleteAsync(item);
            }
        }
    }
}
