
namespace Maui.LocalDB.Sync.Conflicts
{
    public class CustomMergeStrategy<T> : IConflictResolver<T>
    {
        private readonly Func<T, T, T> _mergeFunc;

        public CustomMergeStrategy(Func<T, T, T> mergeFunc)
        {
            _mergeFunc = mergeFunc;
        }

        public T Resolve(T local, T remote)
        {
            return _mergeFunc(local, remote);
        }
    }
}
