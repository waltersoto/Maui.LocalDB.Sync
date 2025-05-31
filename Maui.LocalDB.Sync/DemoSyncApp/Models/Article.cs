using Maui.LocalDB.Sync.Core;
using SQLite;

namespace DemoSyncApp.Models
{
    public class Article : ISyncableEntity<Guid>
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime LastModified { get; set; }
    }
}
