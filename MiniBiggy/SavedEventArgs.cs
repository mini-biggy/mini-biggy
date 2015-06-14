using System;

namespace MiniBiggy {
    public class SavedEventArgs : EventArgs {
        public Exception Exception { get; set; }
        public SavedEventArgs(Exception exception) {
            Exception = exception;
        }
    }
}