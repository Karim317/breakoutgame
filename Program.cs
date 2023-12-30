using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace breakoutgame
{
    public class HashTable<TKey, TValue>
    {
        private const int Capacity = 100; // Initial capacity of the hashtable
        private LinkedList<KeyValuePair<TKey, TValue>>[] buckets;

        public HashTable()
        {
            buckets = new LinkedList<KeyValuePair<TKey, TValue>>[Capacity];
        }

        // Hash function to determine the index in the array
        private int GetHashIndex(TKey key)
        {
            int hash = key.GetHashCode();
            return Math.Abs(hash % Capacity);
        }
        public void Edit(TKey key, TValue newValue)
        {
            int index = GetHashIndex(key);

            if (buckets[index] != null)
            {
                foreach (var pair in buckets[index])
                {
                    if (pair.Key.Equals(key))
                    {
                        // Update the value with the new value
                       // pair.Value = newValue;
                        buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, newValue));
                        return;
                    }
                }
            }

            // If the key is not found, throw an exception
            throw new KeyNotFoundException("The key was not found in the hashtable.");
        }
        // Add a key-value pair to the hashtable
        public void Add(TKey key, TValue value)
        {
            int index = GetHashIndex(key);

            if (buckets[index] == null)
            {
                buckets[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
            }

            foreach (var pair in buckets[index])
            {
                if (pair.Key.Equals(key))
                {
                    throw new ArgumentException("An item with the same key already exists in the hashtable.");
                }
            }

            buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
        }

        // Get the value associated with a key
        public TValue Get(TKey key)
        {
            int index = GetHashIndex(key);

            if (buckets[index] != null)
            {
                foreach (var pair in buckets[index])
                {
                    if (pair.Key.Equals(key))
                    {
                        return pair.Value;
                    }
                }
            }

            throw new KeyNotFoundException("The key was not found in the hashtable.");
        }

        // Remove a key-value pair from the hashtable
        public void Remove(TKey key)
        {
            int index = GetHashIndex(key);

            if (buckets[index] != null)
            {
                var node = buckets[index].First;

                while (node != null)
                {
                    if (node.Value.Key.Equals(key))
                    {
                        buckets[index].Remove(node);
                        return;
                    }

                    node = node.Next;
                }
            }

            throw new KeyNotFoundException("The key was not found in the hashtable.");
        }
        public KeyValuePair<TKey, TValue> FindMaximum()
        {
            KeyValuePair<TKey, TValue> maxPair = default;

            foreach (var bucket in buckets)
            {
                if (bucket != null)
                {
                    foreach (var pair in bucket)
                    {
                        if (maxPair.Equals(default) || Comparer<TKey>.Default.Compare(pair.Key, maxPair.Key) > 0)
                        {
                            maxPair = pair;
                        }
                    }
                }
            }

            if (maxPair.Equals(default))
            {
                throw new InvalidOperationException("The hashtable is empty.");
            }

            return maxPair;
        }

        // Find the minimum value in the hashtable based on the key
        public KeyValuePair<TKey, TValue> FindMinimum()
        {
            KeyValuePair<TKey, TValue> minPair = default;

            foreach (var bucket in buckets)
            {
                if (bucket != null)
                {
                    foreach (var pair in bucket)
                    {
                        if (minPair.Equals(default) || Comparer<TKey>.Default.Compare(pair.Key, minPair.Key) < 0)
                        {
                            minPair = pair;
                        }
                    }
                }
            }

            if (minPair.Equals(default))
            {
                throw new InvalidOperationException("The hashtable is empty.");
            }

            return minPair;
        }
        public List<KeyValuePair<TKey, TValue>> Sort()
        {
            var result = new List<KeyValuePair<TKey, TValue>>();

            foreach (var bucket in buckets)
            {
                if (bucket != null)
                {
                    result.AddRange(bucket);
                }
            }

            result.Sort((x, y) => Comparer<TKey>.Default.Compare(x.Key, y.Key));
            return result;
        }

    }
    public class Player
    {
        public string name { get; set; }
        public int score { get; set; }
    }

    public class Root
    {
        public List<Player> players { get; set; }
    }
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new startscreen());
            HashTable<string, int> myHashTable = new HashTable<string, int>();
            string link = @"C:\Users\reemn\Desktop\p.json.txt";
            WebRequest request = WebRequest.Create(link);
            WebResponse response = request.GetResponse();
            using (Stream datastream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(datastream);
                string responsefromserver = reader.ReadToEnd();
                Root root = JsonConvert.DeserializeObject<Root>(responsefromserver);
                foreach (Player p in root.players)
                {
                    myHashTable.Add(p.name, p.score);
                }
                foreach (Player p in root.players)
                {
                    Console.WriteLine(p.name);
                }
            }
        }
    }
}
