using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace TestMod
{
    [BepInPlugin(MyGuid, PluginName, VersionString)]
    [BepInDependency("com.snmodding.nautilus")]
    public class TestMod : BaseUnityPlugin
    {
        private const string MyGuid = "madscience.testmod";
        private const string PluginName = "Test Mod";
        private const string VersionString = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(MyGuid);

        public static ManualLogSource Log;

        private void Awake()
        {
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }
    }
}