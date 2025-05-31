
namespace Maui.LocalDB.Sync.Conflicts
{
    public class PreferRemoteStrategy<T> : IConflictResolver<T>
    {
        public T Resolve(T local, T remote)
        {
            return remote;
        }
    }
}
