using System;

public class Sample
{
	public Sample()
	{
        EasyPID easyPID = new EasyPID(.5, .2, .1, 120);
        while (true)
        {
            //Get actual value from device
            double actualValue = 100;
            double controlValue = easyPID.TryGetControlSignal(easyPID, actualValue);
        }
    }
}
