using Brutal.Logging;
using KSA;

namespace VehicleDestruction
{
    internal class CollisionDetector
    {
        public static bool Collision(Vehicle vehicle)
        {
            if (vehicle.LastKinematicStates.Situation.HasAnyContact() && vehicle.GetSurfaceSpeed() > 14.0)
            {
                return DestroyVehicle(vehicle);
            }
            else
            {
                return false;
            }
        }

        public static bool DestroyVehicle(Vehicle vehicle)
        {
            var vehicles = Universe.CurrentSystem?.Vehicles;
			if (vehicles != null)
			{
                SendVehicleInfo(vehicle);

                if (Program.ControlledVehicle == vehicle)
                {
                    Program.SetCameraMode(CameraMode.Map);
                    VehicleDestructionMod.CrashOccurred = true;
                }

                // Gets rid of vehicle in Ground Tracking, GameAudio, and the parent Astronomical's Children list
                vehicle.Dispose();

                // Deregister the vehicle from the system's vehicle list
                vehicles.Deregister(vehicle);

                // Disable orbit display for the destroyed vehicle
                vehicle.ShowOrbit = false;

                return true;
            }
            return false;
        }

        public static void SendVehicleInfo(Vehicle vehicle)
        {
            VehicleDestructionMod.LastCrashedVehicle = vehicle;
            VehicleDestructionMod.VehicleCrashSpeed = vehicle.GetSurfaceSpeed();
            VehicleDestructionMod.VehicleCrashId = vehicle.Id;
            VehicleDestructionMod.VehicleCrashedInto = vehicle.Parent;
        }
    }
}
