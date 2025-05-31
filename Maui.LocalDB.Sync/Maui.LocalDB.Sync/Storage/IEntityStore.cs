using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.LocalDB.Sync.Storage
{
    public interface IEntityStore<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task SaveAsync(IEnumerable<T> items);
        Task DeleteAsync(Func<T, bool> predicate);
    }

}
