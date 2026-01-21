using Brutal.GlfwApi;
using Brutal.Logging;
using HarmonyLib;
using KSA;
using System.Runtime.InteropServices;

namespace VehicleDestruction
{
	[HarmonyPatch]
	public static class Patcher
	{
		[HarmonyPatch(typeof(Vehicle), nameof(Vehicle.OnKey))]
		[HarmonyPrefix]
		public static bool KeyInput(RenderCore.Input.GlfwKeyEvent keyEvent)
		{
			GlfwKey key = keyEvent.Key;
			GlfwKeyAction action = keyEvent.Action;

			if (key == GlfwKey.Apostrophe && action == GlfwKeyAction.Release)
			{
				Vehicle? currentVehicle = Program.ControlledVehicle;
                var vehicles = Universe.CurrentSystem?.Vehicles;
				if (currentVehicle != null && vehicles != null)
				{
                    var vehiclesList = vehicles.GetList();
					int index = vehiclesList.IndexOf(currentVehicle) + 1;

					// Gets rid of vehicle in Ground Tracking, GameAudio, and the parent Astronomical's Children list
                    currentVehicle.Dispose();

                    // Deregister the vehicle from the system's vehicle list
                    vehicles.Deregister(currentVehicle);

                    // Disable orbit display for the destroyed vehicle
                    currentVehicle.ShowOrbit = false;

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
                }

                // Return true to continue with the other key checks
                return true;
			}
			return true;
			// Return false to skip all the other key checks
		}
	}
}