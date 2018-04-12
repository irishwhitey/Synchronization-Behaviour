using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Synchronization_Challenge
{
    [TestFixture()]
    class SynchronizationTests
    {
        [Test]
        public void WithoutSynchronizationThingsAreUnpredictabale()
        {
            System.Threading.ThreadPool.SetMinThreads(10,10);
            var tasks = new List<Task>();
            var theClass = new SingleCountingClass();
            var numberOfThreads = 10;
            for (int i = 0; i < numberOfThreads; i++)
            {
                tasks.Add(Task.Run(() => theClass.AddOne()));    
            }
            tasks.ForEach(t => t.Wait());
            Assert.That(theClass.GetCounter(),Is.Not.EqualTo(numberOfThreads));
        }
    }

    public class SingleCountingClass
    {
        private int _counter;

        public void AddOne()
        {
            var _counterValue = _counter;
            Thread.Sleep(250);
            _counter = _counterValue + 1;
        }

        public int GetCounter()
        {
            return _counter;
        }

    }
}
