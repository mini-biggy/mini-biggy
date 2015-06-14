using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniBiggy.Util {
    public static class TimeMachine {
        private static List<CancellationTokenSource> _tokens = new List<CancellationTokenSource>(); 
        private static object _syncRoot = new object();
        private static Func<DateTime> _nowFuncion = () => DateTime.Now;
        private static Func<TimeSpan, TimeSpan> _funcThatReturnsNewDelay = millisecondsDelay => millisecondsDelay;
        public static DateTime Now => _nowFuncion.Invoke();
        public static void OverrideNowWith(Func<DateTime> func) {
            _nowFuncion = func;
        }
        public async static Task Delay(int millisecondsDelay) {
            await Delay(TimeSpan.FromMilliseconds(millisecondsDelay));
        }

        public async static Task Delay(TimeSpan delay) {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            lock (_syncRoot) {
                _tokens.Add(tokenSource);
            }
            try {
                await Task.Delay(_funcThatReturnsNewDelay.Invoke(delay), token);
            }
            catch(TaskCanceledException) { }
            lock (_syncRoot) {
                _tokens.Remove(tokenSource);
            }
        }

        public static int UnblockOneOrMoreDelays() {
            while (_tokens.Count == 0) {
                Task.Delay(10).Wait();
            }
            return UnblockAllDelays();
        }

        public static int UnblockAllDelays() {
            lock (_syncRoot) {
                for (int i = 0; i < _tokens.Count; i++) {
                    _tokens[i].Cancel(false);
                }
                return _tokens.Count;
            }
        }

        public static void OverrideDelayWith(Func<TimeSpan, TimeSpan> funcThatReturnsNewDelay) {
            _funcThatReturnsNewDelay = funcThatReturnsNewDelay;
        }
    }
}