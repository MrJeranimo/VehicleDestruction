using HarmonyLib;
using KSA;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VehicleDestruction
{
    [HarmonyPatch(typeof(UncompressedSave))]
    public class VCLogReader
    {
        [HarmonyPatch("Load")]
        [HarmonyPostfix]
        public static void Postfix(UncompressedSave __instance)
        {
            VehicleDestructionMod.LoadingSave = true;
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                Converters = { new SimTimeConverter() }
            };
            string path = Path.Combine(__instance.Directory.FullName, "VehicleCrashLog.json");
            if (!File.Exists(path))
                return;

            var data = JsonSerializer.Deserialize<VehicleCrashData>(File.ReadAllText(path), options);

            if (data != null)
            {
                VehicleCrashLog.Crashes = data.Crashes ?? new Stack<VehicleCrashInfo>();
                CollisionDetector.CrashSpeedThreshold = data.CrashSpeedThreshold;
                VehicleDestructionMod.tempThreshold = data.CrashSpeedThreshold;
            }

            // Remove crashed vehicles from the current system when loading save
            foreach (var crash in VehicleCrashLog.Crashes)
            {
                Vehicle? tempVehicle = null;
                Universe.CurrentSystem!.Vehicles.TryGet(crash.VehicleId, out tempVehicle);
                if(tempVehicle != null && tempVehicle.Id == crash.VehicleId)
                {
                    CollisionDetector.RemoveVehicle(tempVehicle);
                }
            }

            VehicleDestructionMod.LoadingSave = false;
        }
    }
}
