using System;
using System.Diagnostics;

namespace Synchronization_Challenge
{
    public class TimingHelper
    {
        public static long TimeToRunInMilliseconds(Action stuffToRun)
        {
            var stopWatch1 = new Stopwatch();
            stopWatch1.Start();
            stuffToRun();
            stopWatch1.Stop();
            return stopWatch1.ElapsedMilliseconds;
        }
    }
}