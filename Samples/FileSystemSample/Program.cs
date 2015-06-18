using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniBiggy;
using MiniBiggy.SaveStrategies;

namespace FileSystemSample {
    class Program {

        private static PersistentList<Tweet> _miniBiggy;

        static void Main(string[] args) {
            _miniBiggy = PersistentList.Create<Tweet>(new BackgroundSave(TimeSpan.FromSeconds(3)));
            for (int i = 0; i < 1000; i++) {
                var t = new Tweet { Id = i };
                _miniBiggy.Add(t);
            }
            Write("Done setup: " + _miniBiggy.Count);

            _miniBiggy.Saved += (sender, eventArgs) => {
                Console.WriteLine("Saved! Size: " + _miniBiggy.Count);
            };

            Console.WriteLine("Size: " + _miniBiggy.Count);
            for (int i = 0; i < 100; i++) {
                Add(i, 100);
                Remove(i, 100);    
            }
            Console.ReadLine();
        }

        private static void Add(int id, int num) {
            Task.Run(() => {
                for (int i = 0; i < num; i++) {
                    try {
                        var t = new Tweet { Id = new Random().Next(1000) };
                        _miniBiggy.Add(t);
                        Write(id + "Add");
                    }
                    catch (Exception e) {
                        Write("Error adding");
                    }
                }
            });
        }

        private static void Remove(int id, int num) {
            Task.Run(() => {
                for (int i = 0; i < num; i++) {
                    try {
                        var t = _miniBiggy.Skip(10).First();
                        _miniBiggy.Remove(t);
                        Write(id + "Remove");
                    }
                    catch (Exception ex) {
                        Write("Error removing");
                    }
                }
            });
        }

        private static void Write(string text) {
            Console.WriteLine("-> " + text);
        }

        static void Do(ICollection<Tweet> list) {
            
        }
    }
}
