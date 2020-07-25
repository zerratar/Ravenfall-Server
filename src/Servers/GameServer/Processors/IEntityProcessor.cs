using GameServer.Managers;
using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace GameServer.Processors
{
    public interface IEntityProcessor<T> where T : IEntity
    {
        void Update(T item, IGameSession session, TimeSpan deltaTime);
    }
}