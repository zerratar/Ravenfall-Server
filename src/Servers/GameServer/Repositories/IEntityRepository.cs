using System;
using System.Collections.Generic;

namespace GameServer.Repositories
{
    public interface IEntityRepository<T>
    {
        IReadOnlyList<T> All();
        void Add(T item);
        void Remove(T item);
        T FirstOrDefault(Func<T, bool> predicate);

        void Save();
    }
}
