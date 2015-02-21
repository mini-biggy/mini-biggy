using System;
using MiniBiggy;

namespace FileSystemSample {
    class Program {
        static void Main(string[] args) {
            var t = new Tweet();
            var list = PersistentList.Create<Tweet>();
            list.Add(t);
            Console.WriteLine("Size: " + list.Count);
            Console.ReadLine();
        }
    }
}
