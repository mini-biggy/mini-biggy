using System;

namespace MiniBiggy.SaveStrategies {
    public class SaveOnlyWhenRequested : ISaveStrategy {
        public event EventHandler NotifyUnsolicitedSave;
        public bool ShouldSaveNow() {
            return false;
        }
    }
}