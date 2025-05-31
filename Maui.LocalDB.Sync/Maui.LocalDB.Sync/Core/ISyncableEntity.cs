using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.LocalDB.Sync.Core
{
    public interface ISyncableEntity<TKey>
    {
        TKey Id { get; set; }
        DateTime LastModified { get; set; }
    }
}
