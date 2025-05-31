namespace Maui.LocalDB.Sync.Core
{

    public static class RetryHelper
    {
        public static async Task ExecuteWithRetry(
            Func<Task> operation,
            int maxRetries = 3,
            int delayMs = 500)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    await operation();
                    return;
                }
                catch
                {
                    if (++attempts > maxRetries)
                        throw;
                    await Task.Delay(delayMs * attempts);
                }
            }
        }

        public static async Task<T> ExecuteWithRetry<T>(
            Func<Task<T>> operation,
            int maxRetries = 3,
            int delayMs = 500)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    return await operation();
                }
                catch
                {
                    if (++attempts > maxRetries)
                        throw;
                    await Task.Delay(delayMs * attempts);
                }
            }
        }
    }


}
