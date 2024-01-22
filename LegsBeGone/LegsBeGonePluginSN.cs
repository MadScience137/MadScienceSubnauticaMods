using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MadScienceSubnauticaMods.LegsBeGone.Data;
using Nautilus.Handlers;
using Nautilus.Utility;

namespace MadScienceSubnauticaMods.LegsBeGone
{
    [BepInPlugin(Guid, PluginName, VersionString)]
    [BepInDependency("com.snmodding.nautilus")]
    public class LegsBeGonePluginSN : BaseUnityPlugin
    {
        private const string Guid = "madscience.lagsbegone";
        private const string PluginName = "Legs Be Gone Mod SN";
        private const string VersionString = "2.0.0";

        private static readonly Harmony Harmony = new Harmony(Guid);

        public static ManualLogSource logger;
        public static ModConfig ModConfig;

        private void Awake()
        {
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            logger = Logger;
            ModConfig = OptionsPanelHandler.RegisterModOptions<ModConfig>();
            SaveUtils.RegisterOnStartLoadingEvent(LegDataProvider.Load);
            SaveUtils.RegisterOnSaveEvent(LegDataProvider.Save);
        }
    }
}