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
        public static Vehicle LastCrashedVehicle = Vehicle.CreateBareBones(Universe.CurrentSystem!, "temp");
        public static double VehicleCrashSpeed = 0.0;
        public static string VehicleCrashId = "";
        public static Astronomical VehicleCrashedInto = Vehicle.CreateBareBones(Universe.CurrentSystem!, "temp");

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
            if(CrashOccurred)
            {
                if(ImGui.Begin("Vehicle Crash Report", ref CrashOccurred, ImGuiWindowFlags.AlwaysAutoResize))
                {
                    ImGui.Text($"Vehicle ID: {LastCrashedVehicle.Id}");
                    ImGui.Text($"Crash Speed: {LastCrashedVehicle.GetSurfaceSpeed():F2} m/s");
                    ImGui.Text($"Crashed Into: {VehicleCrashedInto.Id}");
                    ImGui.Separator();
                    var vehicleList = Universe.CurrentSystem?.Vehicles.GetList();
                    if(vehicleList != null && vehicleList.Count > 0)
                    {
                        ImGui.Text("Remaining Vehicles:");
                        foreach (var vehicle in vehicleList)
                        {
                            if(ImGui.Button($"{vehicle.Id} - At: {vehicle.Parent.Id}"))
                            {
                                Program.SetCameraMode(CameraMode.Orbit);
                                Universe.MoveCameraTo(vehicle);
                                CrashOccurred = false;
                            }
                        }
                    }
                    else
                    {
                        ImGui.Text("No remaining vehicles in the system.");
                        if (ImGui.Button("Close"))
                        {
                            CrashOccurred = false;
                        }
                    }
                }
                ImGui.End();
            }
        }

        [ModMenuEntry("Vehicle Destruction")]
        public void OpenLastCrashReport()
        {
            ImGui.MenuItem("View Last Crash Report", ImString.Null, ref CrashOccurred);
        }
    }
}
