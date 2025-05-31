namespace Maui.LocalDB.Sync.Conflicts
{
    public interface IConflictResolver<T>
    {
        T Resolve(T localItem, T remoteItem);
    }
}
