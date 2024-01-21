namespace MadScienceSubnauticaMods.LegsBeGone.Util
{
    public class PillarUtil
    {
        public static bool IsActive(BaseFoundationPiece.Pillar pillar, Base baseObject, BaseFoundationPiece foundation)
        {
            var adjustable = pillar.adjustable;
            var floorDistance = BaseUtils.GetFloorDistance(adjustable.position, adjustable.forward, foundation.maxPillarHeight, baseObject.gameObject);
            var height = floorDistance + 0.01f + foundation.extraHeight;
            return adjustable && floorDistance > -1f && height >= foundation.minHeight;
        }
    }
}