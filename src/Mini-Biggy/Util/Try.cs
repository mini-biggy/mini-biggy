using System;
using System.Threading.Tasks;

namespace MiniBiggy.Util {
    public static class Try {
        public static async Task ThreeTimes(Func<Task> func) {
            await Again(func, 3);
        }

        private static async Task Again(Func<Task> func, int times, int millisecondsBetween = 100) {
            for (int i = 0; i < times; i++) {
                try {
                    await func.Invoke();
                    return;
                }
                catch (Exception) {
                    times--;
                    if (times == 0) {
                        throw;
                    }
                    await Task.Delay(millisecondsBetween);
                    await Again(func, --times);
                }
            }
        }
    }
}