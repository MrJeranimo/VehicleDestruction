using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleDestruction
{
    internal class VehicleCrashData
    {
        public Stack<VehicleCrashInfo> Crashes { get; set; } = new Stack<VehicleCrashInfo>();
        public double CrashSpeedThreshold { get; set; }
    }
}
