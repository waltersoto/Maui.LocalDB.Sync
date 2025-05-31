# Maui.LocalDB.Sync

**Maui.LocalDB.Sync** is a lightweight, modular sync library for .NET MAUI apps that need offline-first functionality. It provides an extensible and testable mechanism to sync remote data with a local SQLite database using a clean and pluggable architecture.

![NuGet](https://img.shields.io/badge/nuget-coming_soon-blue)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 🌟 Features

* ✅ Sync remote data into a local SQLite database
* 🔄 Conflict resolution strategies (Prefer Local, Prefer Remote, Custom Merge)
* 🧹 Optional TTL (time-to-live) data pruning support
* 📡 Incremental sync support (Last-Modified or ETag ready)
* 🔁 Retry logic for remote fetch/save operations 
* ⚙️ Works great with .NET MAUI and SQLite-net-pcl

---

## 📦 Installation

Coming soon via NuGet:

```bash
dotnet add package Maui.LocalDB.Sync
```

---

## 🧩 Basic Usage

### 1. Implement Your Entity

```csharp
public class Article : ISyncableEntity<Guid>
{
    public Guid Id { get; set; }
    public DateTime LastModified { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}
```

### 2. Initialize a SQLiteStore

```csharp
var store = new SQLiteStore<Article, Guid>("path/to/your.db");
```

### 3. Create Your SyncManager

```csharp
var manager = new SyncManager<Article, Guid>(
    fetchRemote: async () => await remoteService.GetArticlesAsync(),
    fetchLocal: store.GetAllAsync,
    saveLocal: store.SaveAsync,
    resolveConflict: new PreferRemoteStrategy<Article>().Resolve
);

var results = await manager.SyncAsync();
```

---

## 🔄 Incremental Sync Support (Advanced)

Supports tracking the last sync timestamp using `ISyncMetadataProvider`:

```csharp
public interface ISyncMetadataProvider<TKey>
{
    Task<DateTime?> GetLastSyncedUtcAsync(TKey scope);
    Task SetLastSyncedUtcAsync(TKey scope, DateTime timestamp);
}
```

Pass it into the `SyncManager` constructor to enable automatic timestamp tracking.

---

## 🧼 TTL Policies (Optional Pruning)

Use `ITtlPolicy<T>` to delete expired records automatically:

```csharp
public class ArticleTtlPolicy : ITtlPolicy<Article>
{
    public bool ShouldDelete(Article article) =>
        article.LastModified < DateTime.UtcNow.AddDays(-30);
}
```

---

## 🔁 Retry & Error Handling

All fetch/save operations can be wrapped with retry logic to improve resilience:

```csharp
await RetryHelper.ExecuteWithRetry(() => store.SaveAsync(items));
```

Each `SyncResult<T>` contains an `Exception? Error` field to track failed operations.

---

## ✅ To Do

* [x] TTL Pruning Support
* [x] Retry + Error Tracking
* [x] Incremental Sync via Metadata
* [ ] NuGet Packaging & Versioning
* [ ] Platform-specific samples (Android, Windows, etc)

---

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

## 💬 Contributing

Contributions and suggestions are welcome! Please open an issue or submit a pull request.

---

## 🙌 Credits

Built with ❤️ for developers who need reliable offline sync in cross-platform apps.

---
