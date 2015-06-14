using System;

namespace MiniBiggy.SaveStrategies {
    public class SaveOnEveryChange : ISaveStrategy {
        public event EventHandler NotifyUnsolicitedSave;
        public bool ShouldSaveNow() {
            return true;
        }
    }
}