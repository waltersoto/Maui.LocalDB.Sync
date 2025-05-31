namespace Maui.LocalDB.Sync.TTL
{
    public interface ITtlPolicy<T>
    {
        bool ShouldDelete(T entity);
    }
}
