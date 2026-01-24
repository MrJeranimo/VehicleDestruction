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

            VehicleCrashLog.Crashes = JsonSerializer.Deserialize<List<VehicleCrashInfo>>(File.ReadAllText(path), options) ?? new List<VehicleCrashInfo>();
        }
    }
}
