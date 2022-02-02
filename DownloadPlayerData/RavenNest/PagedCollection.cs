using System.Collections.Generic;

namespace DownloadPlayerData
{
    public class PagedCollection<T>
    {
        public long TotalSize { get; set; }
        public IReadOnlyList<T> Items { get; set; }
    }
}
