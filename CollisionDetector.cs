using Brutal.Numerics;
using KSA;

namespace VehicleDestruction
{
    public class CollisionDetector
    {
        public static bool ShowPopup = false;
        public static double CrashSpeedThreshold = 14.0;

        public static void CheckCollisions()
        {
            List<Vehicle>? vehicles = Universe.CurrentSystem?.Vehicles.GetList();
            if (vehicles != null)
            {
                for (int firstVehicleIndex = 0; firstVehicleIndex < vehicles.Count; firstVehicleIndex++)
                {
                    Vehicle firstVehicle = vehicles[firstVehicleIndex];

                    // All other vehicles collision check
                    for (int secondVehicleIndex = firstVehicleIndex + 1; secondVehicleIndex < vehicles.Count; secondVehicleIndex++)
                    {
                        Vehicle secondVehicle = vehicles[secondVehicleIndex];
                        if(firstVehicle.Parent == secondVehicle.Parent)
                        {
                            double3 firstVehicleCCIPosition = firstVehicle.GetPositionCci();
                            double3 firstVehicleCCIVelocity = firstVehicle.GetVelocityCci();

                            double3 secondVehicleCCIPosition = secondVehicle.GetPositionCci();
                            double3 secondVehicleCCIVelocity = secondVehicle.GetVelocityCci();

                            double distance = double3.Distance(firstVehicleCCIPosition, secondVehicleCCIPosition);
                            double relativeSpeed = double3.Distance(firstVehicleCCIVelocity, secondVehicleCCIVelocity);

                            if (distance <= firstVehicle.BoundingSphereRadius && relativeSpeed >= CrashSpeedThreshold)
                            {
                                if (firstVehicle.TotalMass > secondVehicle.TotalMass)
                                {
                                    DestroyVehicleVehicle(secondVehicle, firstVehicle, relativeSpeed);
                                }
                                else
                                {
                                    DestroyVehicleVehicle(firstVehicle, secondVehicle, relativeSpeed);
                                }
                            }
                            else if (distance <= secondVehicle.BoundingSphereRadius && relativeSpeed >= CrashSpeedThreshold)
                            {
                                if (firstVehicle.TotalMass > secondVehicle.TotalMass)
                                {
                                    DestroyVehicleVehicle(secondVehicle, firstVehicle, relativeSpeed);
                                }
                                else
                                {
                                    DestroyVehicleVehicle(firstVehicle, secondVehicle, relativeSpeed);
                                }
                            }
                        }
                    }

                    // StellarBody collision check
                    if (firstVehicle.GetRadarAltitude() <= firstVehicle.BoundingSphereRadius && firstVehicle.GetSurfaceSpeed() > CrashSpeedThreshold)
                    {
                        DestroyVehicleStellarBody(firstVehicle);
                    }
                }
            }
        }

        public static void DestroyVehicleStellarBody(Vehicle vehicle)
        {
            VehicleCrashLog.LogCrashStellarBody(vehicle, Universe.GetElapsedSimTime());
            RemoveVehicle(vehicle);
        }

        public static void DestroyVehicleVehicle(Vehicle vehicle, Vehicle crashedInto, double speed)
        {
            VehicleCrashLog.LogCrashVehicle(vehicle, crashedInto, speed, Universe.GetElapsedSimTime());
            RemoveVehicle(vehicle);
        }

        public static void RemoveVehicle(Vehicle vehicle)
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
            }
        }
    }
}
