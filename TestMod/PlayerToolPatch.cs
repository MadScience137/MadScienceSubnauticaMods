using System.Diagnostics;
using HarmonyLib;

namespace TestMod
{
    public class PlayerToolPatch
    {
        [HarmonyPatch(typeof(PlayerTool))]
        internal class PlayerToolPatches
        {
            [HarmonyPatch(nameof(PlayerTool.Awake))]
            [HarmonyPostfix]
            public static void Awake_Postfix(PlayerTool __instance)
            {
                TestMod.Log.LogDebug("Test");
            }
        }
    }
}