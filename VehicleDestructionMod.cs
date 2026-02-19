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
        public static bool LoadingSave = false;
        public static double tempThreshold = CollisionDetector.CrashSpeedThreshold;

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

        [StarMapAfterOnFrame]
        public void CollisionCheck(double currentPlayerTime, double dtPlayer)
        {
            CollisionDetector.CheckCollisions();
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
            ImGui.InputDouble("Crash Speed Threshold", ref tempThreshold);
            if(ImGui.Button("Confirm Change"))
            {
                CollisionDetector.CrashSpeedThreshold = tempThreshold;
            }
        }
    }
}
