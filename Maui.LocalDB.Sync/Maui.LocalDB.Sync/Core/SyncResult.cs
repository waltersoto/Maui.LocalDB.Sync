
namespace Maui.LocalDB.Sync.Core
{
    public class SyncResult<T>
    {
        public T Entity { get; set; }
        public SyncStatus Status { get; set; }
        public Exception? Error { get; set; }

        public SyncResult(T entity, SyncStatus status, Exception? error = null)
        {
            Entity = entity;
            Status = status;
            Error = error;
        }
    }

    public enum SyncStatus
    {
        Synced,
        Updated,
        Inserted,
        Deleted,
        Conflict
    }

}
