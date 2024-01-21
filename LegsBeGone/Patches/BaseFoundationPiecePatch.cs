using HarmonyLib;
using MadScienceSubnauticaMods.LegsBeGone.Data;

namespace MadScienceSubnauticaMods.LegsBeGone.patches
{
    public class BaseFoundationPiecePatch
    {
        [HarmonyPatch(typeof(BaseFoundationPiece))]
        [HarmonyPatch("IBaseAccessoryGeometry.BuildGeometry")]
        internal class BuildGeometry
        {
            [HarmonyPostfix]
            public static void PostFix(BaseFoundationPiece __instance)
            {
                var parentPosition = __instance.GetComponentInParent<BaseCell>().transform.position;
                var baseObject = __instance.GetComponentInParent<Base>();

                if (!LegDataProvider.HasLegs(parentPosition))
                {
                    __instance.pillars.ForEach(pillar => pillar.root.SetActive(false));
                }
            }
        }
    }
}