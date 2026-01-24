using KSA;

namespace VehicleDestruction
{
    public class CollisionDetector
    {
        public static bool ShowPopup = false;

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
                VehicleCrashLog.LogCrash(vehicle, Universe.GetElapsedSimTime());

                // Deregister the vehicle from the system's vehicle list
                vehicles.Deregister(vehicle);

                // Disable orbit display for the destroyed vehicle
                vehicle.ShowOrbit = false;

                if (Program.ControlledVehicle == vehicle)
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
