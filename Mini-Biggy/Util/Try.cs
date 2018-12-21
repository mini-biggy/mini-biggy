using System;
using System.Threading.Tasks;

namespace MiniBiggy.Util
{
    public static class Try
    {
        public static async Task ThreeTimesAsync(Func<Task> func, int increasingMillisecondsBetween = 100)
        {
            await Again(async () =>
            {
                await func.Invoke();
                return 1;
            }
            , 3, increasingMillisecondsBetween);
        }

        public static async Task<T> ThreeTimesAsync<T>(Func<T> func, int increasingMillisecondsBetween = 100)
        {
            return await Again(() =>
            {
                return Task.Run(() => func.Invoke());
            }
            , 3, increasingMillisecondsBetween);
        }

        public static async Task<T> ThreeTimesAsync<T>(Func<Task<T>> func, int increasingMillisecondsBetween = 100)
        {
            return await Again(func, 3, increasingMillisecondsBetween);
        }

        public static async Task<T> Again<T>(Func<Task<T>> func, int times, int increasingMillisecondsBetween = 100)
        {
            try
            {
                return await func.Invoke();
            }
            catch (Exception)
            {
                times--;
                if (times <= 0)
                {
                    throw;
                }
                await Task.Delay(increasingMillisecondsBetween);
                return await Again(func, --times, increasingMillisecondsBetween + increasingMillisecondsBetween);
            }
        }

        public static void SwallowingExceptions(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch
            {
                // ignored
            }
        }
    }
}