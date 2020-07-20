﻿namespace Shinobytes.Ravenfall.RavenNet.Core
{    public interface ILogger
    {
        void Write(string message);
        void WriteLine(string message);
        void Debug(string message);
        void Error(string errorMessage);
    }
}
