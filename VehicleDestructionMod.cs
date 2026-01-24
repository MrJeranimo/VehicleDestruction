using Brutal.ImGuiApi;
using KSA;
using ModMenu;
using StarMap.API;

namespace VehicleDestruction
{
    [StarMapMod]
    public class VehicleDestructionMod
    {
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
                VehicleCrashLog.ShowCrashLog();
            }
            if(CollisionDetector.ShowPopup)
            {
                VehicleCrashLog.CreateCrashReport();
            }
        }

        [ModMenuEntry("Vehicle Destruction")]
        public void OpenLastCrashReport()
        {
            ImGui.MenuItem("Vehicle Crash Log", ImString.Null, ref VehicleCrashLog.ShowWindow);
        }
    }
}
