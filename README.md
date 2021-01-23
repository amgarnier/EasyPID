# EasyPID - Easily create and run a PID controller for your device

## Summary
Easily create a PID device and recieve and output the control signal. You may choose an output range. 
It is important to note that you need to find the current time value in ticks before finding the Process Variable. This is to prevent your read interval to be faster than what your device can read.

## Sample
This sample creates an array of numbers this is our Process Variable or input value.
```C#
 double[] array = new double[200];
            for (int a = 0; a < 200; a++)
            {
                Random random = new Random();
                array[a] = (a + 3) + random.NextDouble();
            }
```
Then we initate a new instance of EasyPID controller and input our Ki, Kp, Kd, Setpoint, and OutputSpeed.

Controllers: Manually tune. Read more in the reference at the bottom of the page
Kp= Proportional controller

Ki= Integral controller

Kd= Derivative controller 

Setpoint = The goal value that you want to set (for example 150 if you want your device to get you to 150F)

OutputSpeed(optional) = This is how fast the controller will read. I recommend using "500" which is 0.5 seconds. You can go faster but it is limited by your processor and the speed of the device output. For this example we will use 1000 for a one second interval

MinmumValue(optional)= Set minum value for the Control Variable. This is set to 0 by default can change to any real number for instance -100,-1,0 etc...

MaximumValue(optional)= Set maimum value for the Control Variable. This is set to 1 by default can change to any real number for instance 100, 1, 1000 etc...
```C#
//create a new instance of EasyPID any assigning an OutputSpeed
            EasyPID easyPID = new EasyPID(.009, 0.05, 0.3, 120, 1000);
```

Now set the current time and the process variable in a while loop
```C#
 long currentTime = DateTime.Now.Ticks;
 double ProcessVariable = array[count];
```

Then run the device by using the `GetControlSignal` command

FullExample:

```C#

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
                double ProcessVariable = array[count];
                double controlVariable = easyPID.GetControlSignal(ProcessVariable, currentTime);
                Console.WriteLine($"Process variable is: {ProcessVariable}, Control Variable is: {controlVariable}");
                ++count;
            }
        }

```



## References 

**PID Wikipedia** [Link](https://en.wikipedia.org/wiki/PID_controller)age
