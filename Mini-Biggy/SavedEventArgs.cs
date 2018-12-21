using System;

namespace MiniBiggy
{
    public class SavedEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
        public bool Success => Exception == null;
        public TimeSpan TimeToSerialize { get; set; }
        public TimeSpan TimeToSave { get; set; }
        public int SizeInBytes { get; set; }

        public SavedEventArgs(Exception exception = null)
        {
            Exception = exception;
        }
    }
}