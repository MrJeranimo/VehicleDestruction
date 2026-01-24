using Brutal.ImGuiApi;
using Brutal.Numerics;
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

        public static void ShowCrashLog()
        {
            if (ImGui.Begin("Vehicle Crash Log", ref ShowWindow, ImGuiWindowFlags.None))
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
                        ImGui.TextWrapped($"Crash Time (from start): {crash.CrashTime.ValueIn(TimeUnit.Years):F0} Years, {crash.CrashTime.ValueIn(TimeUnit.Days)%360:F0} Days, {crash.CrashTime.ValueIn(TimeUnit.Hours)%24:F0} Hours, {crash.CrashTime.ValueIn(TimeUnit.Minutes)%60:F0} Minutes, and {crash.CrashTime.ValueIn(TimeUnit.Seconds)%60:F3} Seconds.");
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

        public static void CreateCrashReport()
        {
            VehicleCrashInfo crash = Crashes.Last();

            // Actually opens the popup
            ImGui.OpenPopup($"{crash.VehicleId} Crashed!");
            
            // Centers the popup
            float2 center = ImGui.GetWindowViewport().GetCenter();
            ImGui.SetNextWindowPos(center, ImGuiCond.Always, new float2(0.5f, 0.5f));

            if (ImGui.BeginPopupModal($"{crash.VehicleId} Crashed!", ref CollisionDetector.ShowPopup, ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoMove))
            {
                ImGui.Text($"Vehicle ID: {crash.VehicleId}");
                ImGui.Text($"Crash Speed: {crash.CrashSpeed:F2} m/s");
                ImGui.Text($"Crashed Into: {crash.CrashedIntoId}");
                ImGui.TextWrapped($"Crash Time (from start): {crash.CrashTime.ValueIn(TimeUnit.Years):F0} Years, {crash.CrashTime.ValueIn(TimeUnit.Days) % 360:F0} Days, {crash.CrashTime.ValueIn(TimeUnit.Hours) % 24:F0} Hours, {crash.CrashTime.ValueIn(TimeUnit.Minutes) % 60:F0} Minutes, and {crash.CrashTime.ValueIn(TimeUnit.Seconds) % 60:F3} Seconds.");
                ImGui.Separator();
                ImGui.Text("Select a new vehicle control");
                foreach(var vehicle in Universe.CurrentSystem!.Vehicles.GetList())
                {
                    if(ImGui.Button(vehicle.Id))
                    {
                        Universe.MoveCameraTo(vehicle);
                        CollisionDetector.ShowPopup = false;
                    }
                    ImGui.SameLine();
                }
            }
            ImGui.EndPopup();
        }
    }
}
