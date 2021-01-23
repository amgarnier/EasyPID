using System;
using System.IO;

namespace Controller.EasyPID
{        /// <summary>
         /// Create a PID device called Easy PID
         /// </summary>
    public class EasyPID
    {
        /// <summary>
        /// Proportional Gain Factor
        /// </summary>
        public double Kp { get; private set; }

        /// <summary>
        /// Integral Gain Factor
        /// </summary>
        public double Ki { get; private set; }

        /// <summary>
        /// Derivative Gain Factor
        /// </summary>
        public double Kd { get; private set; }

        /// <summary>
        /// Goal Value
        /// </summary>
        public double Goal { get; private set; }

        /// <summary>
        /// Goal Value
        /// </summary>
        public long OutputSpeed { get; }

        /// <summary>
        /// Minimum output value that you want this controller to output
        /// </summary>
        public long MinOutput { get; }

        /// <summary>
        /// Maximum output value that you want this controller to output
        /// </summary>
        public long MaxOutput { get; }

        /// <summary>
        /// Residual error from last Cycle
        /// </summary>
        private double errorResidual = 0;

        public double ErrorResidual
        {
            get { return errorResidual; }
            set { errorResidual = value; }
        }

        /// <summary>
        /// Derivative of the function
        /// </summary>
        public double derivative { get; private set; }

        /// <summary>
        /// Error value currently
        /// </summary>
        public double error { get; private set; }

        /// <summary>
        /// Control variable
        /// </summary>
        public double controlVariable { get; private set; }

        /// <summary>
        /// Change in time
        /// </summary>
        public double dT { get; private set; }

        /// <summary>
        /// Previous Error
        /// </summary>
        public double errorPrevious { get; private set; }

        /// <summary>
        /// Interval in MilliSeconds
        /// </summary>
        public long Interval { get; private set; }

        /// <summary>
        /// Creates a new instance of the EasyPID Controller.
        /// </summary>
        /// <param name="Kp">Proportional Gain</param>
        /// <param name="Ki">Proportional Gain</param>
        /// <param name="Kd">Derivative Gain</param>
        /// <param name="Goal">Goal Value</param>
        /// <param name="OutputSpeed">The speed in milliseconds to get your output</param>
        /// <param name="MinOutput">Minimum Output Value of Controller</param>
        /// <param name="MaxOutput">Maximum Output Value of Controller</param>
        public EasyPID(double Kp = 0.0, double Ki = 0.0, double Kd = 0.0, double Goal = 0.0, long OutputSpeed = 1000, long MinOutput = 0, long MaxOutput = 1)
        {
            this.Kp = Kp;
            this.Ki = Kp;
            this.Kd = Kp;
            this.Goal = Goal;
            this.OutputSpeed = OutputSpeed;
            this.MinOutput = MinOutput;
            this.MaxOutput = MaxOutput;
            this.OutputSpeed = OutputSpeed;
            if (OutputSpeed < 1)
            {
                throw new IOException("Interval is set too low please update to be above 1 ms");
            }
            if (MinOutput > MaxOutput)
            {
                throw new IOException("Minimum output must be greater than maximum output");
            }
        }

        /// <summary>
        /// Creates a new instance of the EasyPID Controller. With an output from 0 to 1
        /// </summary>
        /// <param name="Kp">Proportional Gain</param>
        /// <param name="Ki">Proportional Gain</param>
        /// <param name="Kd">Derivative Gain</param>
        /// <param name="Goal">Goal Value</param>
        /// <param name="OutputSpeed">The speed in milliseconds to get your output</param>
        public EasyPID(double Kp = 0.0, double Ki = 0.0, double Kd = 0.0, double Goal = 0.0, long OutputSpeed = 1000)
        {
            this.Kp = Kp;
            this.Ki = Kp;
            this.Kd = Kp;
            this.Goal = Goal;
            this.OutputSpeed = OutputSpeed;
            this.OutputSpeed = OutputSpeed;
            if (OutputSpeed < 1)
            {
                throw new IOException("Interval is set too low please update to be above 1 ms");
            }
            this.MinOutput = 0;
            this.MaxOutput = 1;
        }

        /// <summary>
        /// Gets the signal output of the controller. This value is the percentage of the controller from 0 to 1
        /// </summary>
        /// <returns>
        /// Signal output in value between MinOutput and MaxOutput default is 0 to 1
        /// </returns>
        /// <param name="ActualValue">Actual value to input</param>
        /// <param name="currentTime">Current Time in ticks</param>
        public double GetControlSignal(double ActualValue, long currentTime)
        {
            long intervalTicks = this.OutputSpeed * 10000;
            if (DateTime.Now.Ticks > (currentTime + intervalTicks))
            {
                throw new IOException("Interval is set too low please increase to a value that is slower then the output of your signal");
            }
            var errorValuedT = this.Goal - ActualValue;
            error = this.Goal - ActualValue;
            errorResidual += (error * this.OutputSpeed / 1000);
            if (errorResidual > this.MaxOutput)
            {
                errorResidual = this.MaxOutput;
            }
            derivative = (error - errorValuedT) / this.OutputSpeed / 1000;
            if (Double.IsNaN(derivative))
            {
                derivative = 0;
            }
            while (DateTime.Now.Ticks < (currentTime + intervalTicks))
            {
            }
            controlVariable = (this.Kp * error) + (this.Ki * errorResidual) + (Kd * derivative);
            if (controlVariable <= this.MinOutput)
            {
                controlVariable = this.MinOutput;
            }
            if (controlVariable >= this.MaxOutput)
            {
                controlVariable = this.MaxOutput;
            }
            errorValuedT = error;
            return controlVariable;
        }
    }
}