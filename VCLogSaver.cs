using HarmonyLib;
using KSA;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VehicleDestruction
{
    [HarmonyPatch(typeof(UniverseData))]
    public class VCLogSaver
    {
        [HarmonyPatch("WriteTo", new Type[] { typeof(DirectoryInfo) })]
        [HarmonyPostfix]
        public static void Postfix(DirectoryInfo directory)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                Converters = { new SimTimeConverter() }
            };

            var data = new VehicleCrashData
            {
                Crashes = VehicleCrashLog.Crashes,
                CrashSpeedThreshold = CollisionDetector.CrashSpeedThreshold
            };

            File.WriteAllText(Path.Combine(directory.FullName, "VehicleCrashLog.json"), JsonSerializer.Serialize(data, options));
        }
    }
}