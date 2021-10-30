
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCounterStream.Services
{
    public class CounterStore
    {
        public List<int> CounterHistory { get; set; } = new();

        public void AddValue(int value)
        {
            if (CounterHistory.Count >= 10)
                CounterHistory = CounterHistory.TakeLast(9).ToList();
            CounterHistory.Add(value);
        }
    }
}
