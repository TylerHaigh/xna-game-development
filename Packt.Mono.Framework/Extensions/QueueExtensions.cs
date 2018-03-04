using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packt.Mono.Framework.Extensions
{
    public static class QueueExtensions
    {
        public static void EnqueueRange<T>(this Queue<T> source, IEnumerable<T> range)
        {
            foreach (var r in range)
            {
                source.Enqueue(r);
            }
        }
    }
}
