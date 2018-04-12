//using System;
//using System.CodeDom;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using Moq;
//using NUnit.Framework;

//namespace Synchronization_Challenge
//{
//    [TestFixture]
//    public class ChallengeOne
//    {
//        [Test]
//        public void ItShouldCompleteQuickly()
//        {
//            var fireAndForgetMock = new Mock<IDoStuff>();
//            fireAndForgetMock.Setup(x => x.Notify()).Callback(() => { System.Threading.Thread.Sleep(4000); });
//            var sw = new Stopwatch();
//            var sut = new SpecialClass(fireAndForgetMock.Object);
//            sw.Start();
//            sut.Submit();
//            sw.Stop();
//            Assert.That(sw.ElapsedMilliseconds, Is.LessThan(TimeSpan.FromSeconds(5).TotalMilliseconds));
//            fireAndForgetMock.Verify(x=>x.Notify());

//        }

//        [Test]
//        public void ItShouldCompleteQuickly()
//        {
//            var sw = new Stopwatch();
//            var sut = new SpecialClass();
//            sw.Start();
//            sut.Submit2();
//            sw.Stop();

//        }
//    }

//    public interface IDoStuff
//    {
//        void Notify();
//    }

//    public class SpecialClass
//    {
//        private readonly IDoStuff _doesStuff;

//        public SpecialClass(IDoStuff doesStuff=null)
//        {
//            _doesStuff = doesStuff;
//        }
//        public void Submit()
//        {
//            FourSecondExternalFireAndForgetCall();
//            System.Threading.Thread.Sleep(3000);
//        }

//        private void FourSecondExternalFireAndForgetCall()
//        {
//            _doesStuff.Notify();
//        }
//    }
//}
