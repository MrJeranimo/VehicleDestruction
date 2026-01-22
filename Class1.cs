using HarmonyLib;
using KSA;
using StarMap.API;

namespace VehicleDestruction
{
    [StarMapMod]
    public class Main
    {
        public readonly Harmony MHarmony = new Harmony("VehicleDestruction");

        [StarMapAllModsLoaded]
        public void OnFullyLoaded()
        {
            MHarmony.PatchAll(typeof(Main).Assembly);
        }

        [StarMapUnload]
        public void OnUnload()
        {
            MHarmony.UnpatchAll(nameof(Main));
        }

        [StarMapAfterGui]
        public void CollisionCheck(double dt)
        {
            if (Program.ControlledVehicle != null)
            {
                CollisionDetector.Collision(Program.ControlledVehicle);
            }
        }
    }
}
