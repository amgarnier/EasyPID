using Controller.EasyPID;
using System;

namespace Sample
{
    internal class Sample
    {
        private static void Main(string[] args)
        {
            //Example creates an array of numbers to run the controller against
            // Setup an array of numbers to test the controller with this starts at 0 and increases by 3 plus a random number
            int count = 0;
            double[] array = new double[200];
            for (int a = 0; a < 200; a++)
            {
                Random random = new Random();
                array[a] = (a + 3) + random.NextDouble();
            }
            //create a new instance of EasyPID
            EasyPID easyPID = new EasyPID(.009, 0.05, 0.3, 120, 100);

            while (count < 200)
            {
                //Set timer then get actual value from device
                //You must set currentTime before the actualValue, otherwise your device might not read properly
                long currentTime = DateTime.Now.Ticks;
                double actualValue = array[count];
                double controlValue = easyPID.GetControlSignal(actualValue, currentTime);
                Console.WriteLine($"Actual value is: {actualValue}, Control value is: {controlValue}");
                ++count;
            }
        }
    }
}