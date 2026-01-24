using Brutal.ImGuiApi;
using HarmonyLib;
using KSA;
using ModMenu;
using StarMap.API;

namespace VehicleDestruction
{
    [StarMapMod]
    public class VehicleDestructionMod
    {
        public readonly Harmony MHarmony = new Harmony("VehicleDestruction");

        [StarMapAllModsLoaded]
        public void Load()
        {
            MHarmony.PatchAll(typeof(VehicleDestructionMod).Assembly);
        }

        [StarMapUnload]
        public void Unload()
        {
            MHarmony.UnpatchAll(nameof(VehicleDestructionMod));
        }

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
