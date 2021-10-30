using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCounterStream.Worker
{
    public class CounterEventArgs: EventArgs
    {
        public int Counter { get; set; }
    }
}
