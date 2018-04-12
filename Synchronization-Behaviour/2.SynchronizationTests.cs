using System.Collections.Generic;
using System.Linq;
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
            var tasks = new List<Task>();
            var theClass = new SingleCountingClass();
            var numberOfThreads = 10;
            for (int i = 0; i < numberOfThreads; i++)
            {
                tasks.Add(Task.Run(() => theClass.AddOne()));    
            }
            tasks.ForEach(t => t.Wait());
            Assert.That(theClass.GetCounterWithLock(),Is.Not.EqualTo(numberOfThreads));
        }

        [Test]
        public void WithSynchronizationThingsArePredictabale()
        {
            var tasks = new List<Task>();
            var theClass = new SingleCountingClass();
            var numberOfThreads = 10;
            for (int i = 0; i < numberOfThreads; i++)
            {
                tasks.Add(Task.Run(() => theClass.AddOneWithLock()));
            }
            tasks.ForEach(t => t.Wait());
            Assert.That(theClass.GetCounterWithLock(), Is.EqualTo(numberOfThreads));
        }
    }
}
