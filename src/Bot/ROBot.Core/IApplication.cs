using System;

namespace ROBot
{
    public interface IApplication : IDisposable
    {
        void Run();
        void Shutdown();
    }
}