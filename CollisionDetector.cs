using KSA;

namespace VehicleDestruction
{
    public class CollisionDetector
    {
        public static bool ShowPopup = false;
        public static double CrashSpeedThreshold = 14.0;

        public static bool Collision(Vehicle vehicle)
        {
            if (vehicle.GetRadarAltitude() <= vehicle.BoundingSphereRadius && vehicle.GetSurfaceSpeed() > CrashSpeedThreshold)
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
            VehicleCrashLog.LogCrash(vehicle, Universe.GetElapsedSimTime());
            return RemoveVehicle(vehicle);
        }

        public static bool RemoveVehicle(Vehicle vehicle)
        {
            var vehicles = Universe.CurrentSystem?.Vehicles;
            if (vehicles != null)
            {
                vehicle.StopThrust();

                // Deregister the vehicle from the system's vehicle list
                vehicles.Deregister(vehicle);

                // Disable orbit display for the destroyed vehicle
                vehicle.ShowOrbit = false;

                if (Program.ControlledVehicle == vehicle && !VehicleDestructionMod.LoadingSave)
                {
                    ShowPopup = true;
                }

                // Gets rid of vehicle in Ground Tracking, GameAudio, and the parent Astronomical's Children list
                vehicle.Dispose();

                return true;
            }
            return false;
        }
    }
}
