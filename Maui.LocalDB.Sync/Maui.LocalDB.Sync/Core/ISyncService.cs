using Maui.LocalDB.Sync.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.LocalDB.Sync.Storage
{
    public interface ISyncService<T>
    {
        Task<IEnumerable<T>> FetchRemoteAsync();
        Task<IEnumerable<T>> FetchLocalAsync();
        Task SaveLocalAsync(IEnumerable<T> items);
        Task<IEnumerable<SyncResult<T>>> SyncAsync();
    }

}
