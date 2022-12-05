using HarmonyLib;
using MadScienceSubnauticaMods.LegsBeGone.Data;

namespace MadScienceSubnauticaMods.LegsBeGone.patches
{
    public class BasePatch
    {
        [HarmonyPatch(typeof(Base))]
        [HarmonyPatch(nameof(Base.BuildPillars))]
        internal class BuildPillars
        {
            [HarmonyPrefix]
            public static bool Prefix(Base __instance)
            {
                if (__instance.isGhost)
                {
                    return false;
                }

                Int3.Bounds bounds = __instance.Bounds;
                Int3 mins = bounds.mins;
                Int3 maxs = bounds.maxs;
                Int3 cell = default(Int3);
                for (int i = mins.z; i <= maxs.z; i++)
                {
                    cell.z = i;
                    for (int j = mins.x; j <= maxs.x; j++)
                    {
                        cell.x = j;
                        int k = mins.y;
                        while (k <= maxs.y)
                        {
                            cell.y = k;
                            if (__instance.GetCell(cell) != Base.CellType.Empty)
                            {
                                var foundation = __instance.GetCellObject(cell).GetComponentInChildren<BaseFoundationPiece>();

                                if (foundation != null)
                                {
                                    var parentPosition = foundation.GetComponentInParent<BaseCell>().transform.position;

                                    if (!LegDataProvider.HasLegs(parentPosition))
                                    {
                                        foundation.pillars.ForEach(pillar => pillar.root.SetActive(false));
                                    }
                                    else
                                    {
                                        foundation.OnGenerate();
                                    }
                                }

                                break;
                            }

                            k++;
                        }
                    }
                }

                return false;
            }
        }
    }
}