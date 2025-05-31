
namespace Maui.LocalDB.Sync.Conflicts
{
    public class PreferLocalStrategy<T> : IConflictResolver<T>
    {
        public T Resolve(T local, T remote)
        {
            return local;
        }
    }
}
