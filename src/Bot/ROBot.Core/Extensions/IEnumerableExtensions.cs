using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ROBot.Core.Extensions
{
    public static class ListExtensions
    {
        public static Task ForEachAsync<T>(this IReadOnlyList<T> source, Func<T, Task> action)
        {
            return Task.WhenAll(source.Select(action));
        }
    }

    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source.Where(x => !predicate(x));
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (action == null) throw new ArgumentNullException(nameof(action));
            foreach (var item in items) action(item);
        }
    }

    public static class FileInfoExtensions
    {
        public static bool TryDelete(this FileInfo file)
        {
            try
            {
                file.Delete();
                return true;
            }
            catch
            {
            }

            return false;
        }
    }
}
