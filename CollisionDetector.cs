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
                DefaultCategory.Log.Info($"{vehicle.Id} has crashed while going {vehicle.GetSurfaceSpeed():F2}m/s");
                var vehiclesList = vehicles.GetList();
				int index = vehiclesList.IndexOf(vehicle) + 1;

				// Gets rid of vehicle in Ground Tracking, GameAudio, and the parent Astronomical's Children list
                vehicle.Dispose();

                // Deregister the vehicle from the system's vehicle list
                vehicles.Deregister(vehicle);

                // Disable orbit display for the destroyed vehicle
                vehicle.ShowOrbit = false;

				// If there are no vehicles left, move the camera to the first object in the system
				if (vehicles.GetList().Count == 0)
                {
                    Universe.MoveCameraTo(Universe.CurrentSystem?.All.GetList()[0]!);
                }
                // Go to the next vehicle in the list
                else if (index < vehiclesList.Count - 1)
				{
                    Program.GetMapController().Camera.SetFollow(vehiclesList[index], true);
                    Universe.MoveCameraTo(vehiclesList[index]);
				}
                // Wrap around to the first vehicle if at the end of the list
                else if (index >= vehiclesList.Count - 1) 
				{
					index = 0;
                    Program.GetMapController().Camera.SetFollow(vehiclesList[index], true);
                    Universe.MoveCameraTo(vehiclesList[index]);
                }
                return true;
            }
            return false;
        }
    }
}
