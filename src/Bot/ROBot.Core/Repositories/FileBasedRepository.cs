using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace ROBot.Core.Repositories
{
    public abstract class FileBasedRepository<TInterface, TImpl>
        : FileBasedRepository<TInterface> where TImpl : TInterface
    {
        protected FileBasedRepository(string storageFile) : base(storageFile)
        {
        }

        protected override void LoadRepository()
        {
            lock (Mutex)
            {
                if (!System.IO.File.Exists(StorageFile))
                {
                    return;
                }

                var data = System.IO.File.ReadAllText(StorageFile);
                var items = JsonConvert.DeserializeObject<List<TImpl>>(data) ?? new List<TImpl>();
                this.Items = items.Cast<TInterface>().ToList();
            }
        }
    }

    public abstract class FileBasedRepository<T> : IRepository<T>
    {
        protected readonly string StorageFile;
        protected readonly string StoragePackupPath;
        protected readonly object Mutex = new object();
        protected List<T> Items = new List<T>();

        private int saveCounter;

        protected FileBasedRepository(string storageFile)
        {
            this.StorageFile = storageFile;
            this.StoragePackupPath =
                System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(this.StorageFile),
                    "backups",
                    System.IO.Path.GetFileName(this.StorageFile));
            LoadRepository();
        }

        public T Store(T item)
        {
            lock (Mutex)
            {
                var index = this.Items.FindIndex(x => x.Equals(item) || x.GetHashCode() == item.GetHashCode() && item.GetType() == x.GetType());
                if (index != -1)
                {
                    this.Items[index] = item;
                }
                else
                {
                    this.Items.Add(item);
                }
            }

            return item;
        }

        public void Remove(Predicate<T> predicate)
        {
            lock (Mutex)
            {
                Items.RemoveAll(predicate);
            }
        }

        public void Save() => SaveRepository();

        private void SaveRepository()
        {
            lock (Mutex)
            {
                var data = JsonConvert.SerializeObject(this.Items);
                System.IO.File.WriteAllText(StorageFile, data);
                var nextValue = Interlocked.Increment(ref saveCounter);
                if (string.IsNullOrEmpty(data) || data == "{}") return;
                if (nextValue % 30 == 0)
                {
                    var t = DateTime.Now;
                    var dir = System.IO.Path.Combine(Path.GetDirectoryName(StoragePackupPath), $"{t:MM-dd}");
                    if (!Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);

                    var backupFile = System.IO.Path.Combine(dir,
                        System.IO.Path.GetFileName(StoragePackupPath) + $"-{t:yyyy-MM-dd_hh-mm-ss}.bup");

                    System.IO.File.WriteAllText(backupFile, data);
                }
            }
        }

        protected virtual void LoadRepository()
        {
            lock (Mutex)
            {
                if (!System.IO.File.Exists(StorageFile))
                {
                    return;
                }

                var data = System.IO.File.ReadAllText(StorageFile);
                this.Items = JsonConvert.DeserializeObject<List<T>>(data) ?? new List<T>();
            }
        }

        public IReadOnlyList<T> All()
        {
            lock (Mutex)
            {
                return this.Items;
            }
        }
    }
}