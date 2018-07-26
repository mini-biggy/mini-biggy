using System;
using System.Threading.Tasks;
using MiniBiggy.Util;

namespace MiniBiggy.SaveStrategies {
    public class BackgroundSave : ISaveStrategy {
        public event EventHandler NotifyUnsolicitedSave;
        public static TimeSpan DefaultIntervalBetweenSaves = TimeSpan.FromSeconds(2);
        public bool IsDirty { get; set; }
        public TimeSpan IntervalBetweenSaves { get; set; }

        public BackgroundSave(TimeSpan interval) {
            IntervalBetweenSaves = interval;
            Task.Run(async () => await Loop());
        }

        private async Task Loop() {
            while (true) {
                await TimeMachine.Delay(IntervalBetweenSaves);
                if (!IsDirty) {
                    continue;
                }
                try {
                    OnNotifySave();
                    IsDirty = false;
                }
                catch {
                    await TimeMachine.Delay(IntervalBetweenSaves);
                }
            }
        }

        public bool ShouldSaveNow() {
            IsDirty = true;
            return false;
        }

        protected virtual void OnNotifySave() {
            NotifyUnsolicitedSave?.Invoke(this, EventArgs.Empty);
        }
    }
}