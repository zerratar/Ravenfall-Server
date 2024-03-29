﻿using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Server
{
    public interface IStreamBotFactory
    {
        IStreamBot Create(PlayerConnection connection, User botUser);
    }

}
