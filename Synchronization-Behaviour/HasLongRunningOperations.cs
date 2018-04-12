namespace Synchronization_Challenge
{
    public class HasLongRunningOperations
    {
        public void ThreeSecondOperation()
        {
            System.Threading.Thread.Sleep(3000);
        }

        public void TwoSecondOperation()
        {
            System.Threading.Thread.Sleep(2000);
        }
    }
}