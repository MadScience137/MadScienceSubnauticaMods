using System.Reflection;
using HarmonyLib;
using MadScienceSubnauticaMods.LegsBeGone.Data;
using QModManager.API.ModLoading;
using QModManager.Utility;
using SMLHelper.V2.Handlers;

namespace MadScienceSubnauticaMods.LegsBeGone
{
    [QModCore]
    public static class QMod
    {
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        [QModPatch]
        public static void Patch()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var modName = ($"madsciencemods_{assembly.GetName().Name}");

            Logger.Log(Logger.Level.Info, $"Patching {modName}");

            var harmony = new Harmony(modName);
            harmony.PatchAll(assembly);

            var saveData = SaveDataHandler.Main.RegisterSaveDataCache<LegData>();

            saveData.OnFinishedLoading += (sender, e) =>
            {
                if (!(e.Instance is LegData data)) return;

                LegDataProvider.LoadFromArray(data.DisabledLegs);
                Logger.Log(Logger.Level.Debug, $"Loaded {LegDataProvider.Count} pieces of leg data.");
            };

            saveData.OnStartedSaving += (sender, e) =>
            {
                if (!(e.Instance is LegData data)) return;

                data.DisabledLegs = LegDataProvider.ToArray();
                Logger.Log(Logger.Level.Debug, $"Saved {data.DisabledLegs.Length} pieces of leg data.");
            };

            Logger.Log(Logger.Level.Info, "Patched successfully!");
        }
    }
}