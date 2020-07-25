using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GameServer.Repositories
{
    public class JsonBasedRepository<T> : IEntityRepository<T>
    {
        private const string RepositoryFolder = "repositories";
        private readonly object mutex = new object();
        private readonly string repositoryFile;
        private List<T> itemSource;

        public JsonBasedRepository()
        {
            repositoryFile = typeof(T).FullName + ".json";
            LoadRepository();
        }

        public JsonBasedRepository(string file)
        {
            repositoryFile = file;
            LoadRepository();
        }

        public IReadOnlyList<T> All()
        {
            lock (mutex)
            {
                return this.itemSource;
            }
        }

        public void Add(T item)
        {
            lock (mutex)
            {
                itemSource.Add(item);
                Save();
            }
        }
        public void Remove(T item)
        {
            lock (mutex)
            {
                itemSource.Remove(item);
                Save();
            }
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            lock (mutex)
            {
                return this.itemSource.FirstOrDefault(predicate);
            }
        }

        public void Save()
        {
            lock (mutex)
            {
                var file = GetRepositoryFilePath();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(this.itemSource);
                System.IO.File.WriteAllText(file, data);
            }
        }

        private void LoadRepository()
        {
            lock (mutex)
            {
                var file = GetRepositoryFilePath();
                if (!System.IO.File.Exists(file))
                {
                    this.itemSource = new List<T>();
                    return;
                }

                var data = System.IO.File.ReadAllText(file);
                this.itemSource = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(data);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetRepositoryFilePath()
        {
            return System.IO.Path.Combine(RepositoryFolder, repositoryFile);
        }
    }
}
