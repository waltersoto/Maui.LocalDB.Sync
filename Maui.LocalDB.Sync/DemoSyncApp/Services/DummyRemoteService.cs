using DemoSyncApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoSyncApp.Services
{

    public class DummyRemoteService
    {
        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            await Task.Delay(500); // Simulated delay
            return new List<Article>
            {
                new Article
                {
                    Id = Guid.NewGuid(),
                    Title = "Welcome to MAUI Sync",
                    Content = "This is a dummy remote article.",
                    LastModified = DateTime.UtcNow
                }
            };
        }
    }

}
