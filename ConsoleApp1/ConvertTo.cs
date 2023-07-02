using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public static class ConvertTo
    {
        public static List<List<T>> ChunkInto<T>(this IList<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static string ToDateTime(this string datestring, string format="ddd dd") 
        {
            var dstring = datestring;

            if (System.DateTime.TryParse(datestring, out System.DateTime dTime))
            {
                dstring = dTime.ToString(format);
            }

            return dstring;
        }

    }
}
