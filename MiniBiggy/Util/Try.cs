using System;
using System.Threading.Tasks;

namespace MiniBiggy.FileSystem {
    public static class Try {
        public static async Task ThreeTimes(Func<Task> func) {
            await Again(func, 3);
        }

        private static async Task Again(Func<Task> func, int times) {
            for (int i = 0; i < times; i++) {
                try {
                    await func.Invoke();
                }
                catch {
                    if (times < 0) {
                        throw;
                    }
                    await Again(func, --times);
                }
            }
        }
    }
}