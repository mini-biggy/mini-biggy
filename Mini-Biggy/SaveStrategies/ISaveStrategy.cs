using System;

namespace MiniBiggy.SaveStrategies
{
    public interface ISaveStrategy
    {
        event EventHandler NotifyUnsolicitedSave;

        bool ShouldSaveNow();
    }
}