using System;
using System.Collections.Generic;

namespace MiniBiggy
{
    public class PersistedEventArgs<T> : EventArgs
    {
        public List<T> Items { get; set; }

        public PersistedEventArgs()
        {
            Items = new List<T>();
        }

        public PersistedEventArgs(List<T> items)
        {
            Items = items;
        }
    }
}