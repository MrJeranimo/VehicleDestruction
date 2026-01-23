using Brutal.ImGuiApi;
using KSA;

namespace VehicleDestruction
{
    public struct VehicleCrashInfo
    {
        public string VehicleId;
        public double CrashSpeed;
        public string CrashedIntoId;
        public SimTime CrashTime;
    }

    public class VehicleCrashLog
    {
        public static List<VehicleCrashInfo> Crashes = new List<VehicleCrashInfo>();
        public static bool ShowWindow = false;

        public static void CreateCrashWindow()
        {
            if (ImGui.Begin("Vehicle Crash Log", ref ShowWindow, ImGuiWindowFlags.AlwaysAutoResize))
            {
                if (Crashes.Count == 0)
                {
                    ImGui.Text("No Vehicles crashed yet.");
                }
                else
                {
                    foreach (var crash in Crashes)
                    {
                        ImGui.Text($"Vehicle ID: {crash.VehicleId}");
                        ImGui.Text($"Crash Speed: {crash.CrashSpeed:F2} m/s");
                        ImGui.Text($"Crashed Into: {crash.CrashedIntoId}");
                        ImGui.Text("Crash Time Below does not work properly. Sorry");
                        ImGui.TextWrapped($"Crash Time (from start): {crash.CrashTime.ValueIn(TimeUnit.Days):F0} Days, {crash.CrashTime.ValueIn(TimeUnit.Hours):F0} Hours, {crash.CrashTime.ValueIn(TimeUnit.Minutes):F0} Minutes, and {crash.CrashTime.ValueIn(TimeUnit.Seconds):F3} Seconds.");
                        ImGui.Separator();
                    }
                }
            }
            ImGui.End();
        }

        public static void LogCrash(Vehicle vehicle, SimTime crashTime)
        {
            VehicleCrashInfo crashInfo = new VehicleCrashInfo
            {
                VehicleId = vehicle.Id,
                CrashSpeed = vehicle.GetSurfaceSpeed(),
                CrashedIntoId = vehicle.Parent.Id,
                CrashTime = crashTime
            };
            Crashes.Add(crashInfo);
        }
    }
}
