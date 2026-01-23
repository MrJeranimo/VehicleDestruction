using Brutal.ImGuiApi;
using KSA;
using ModMenu;
using StarMap.API;

namespace VehicleDestruction
{
    [StarMapMod]
    public class VehicleDestructionMod
    {
        public static bool CrashOccurred = false;
        public static Vehicle LastCrashedVehicle = Vehicle.CreateBareBones(Universe.CurrentSystem!, "AEbtf4");
        public static double VehicleCrashSpeed = 0.0;
        public static string VehicleCrashId = "";
        public static Astronomical VehicleCrashedInto = Vehicle.CreateBareBones(Universe.CurrentSystem!, "AEbtf4");

        [StarMapAfterGui]
        public void CollisionCheck(double dt)
        {
            var vehicles = Universe.CurrentSystem?.Vehicles.GetList();
            if(vehicles != null)
            {
                foreach (var vehicle in vehicles)
                {
                    if (CollisionDetector.Collision(vehicle))
                    {
                        break;
                    }
                }
            }
        }

        [StarMapBeforeGui]
        public void CreateCrashReport(double dt)
        {
            if(VehicleCrashLog.ShowWindow)
            {
                VehicleCrashLog.CreateCrashWindow();
            }
        }

        [ModMenuEntry("Vehicle Destruction")]
        public void OpenLastCrashReport()
        {
            ImGui.MenuItem("Vehicle Crash Log", ImString.Null, ref VehicleCrashLog.ShowWindow);
        }
    }
}
