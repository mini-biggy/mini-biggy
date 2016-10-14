using System;

namespace MiniBiggy {
    public class SavedEventArgs : EventArgs {
        public Exception Exception { get;}
        public bool Success => Exception == null;
        public SavedEventArgs(Exception exception = null) {
            Exception = exception;
        }
    }
}