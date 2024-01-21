namespace MadScienceSubnauticaMods.LegsBeGone.Extensions
{
    public static class HandReticleExtension
    {
        public static void AppendTextRaw(this HandReticle instance, HandReticle.TextType type, string text)
        {
            switch (type)
            {
                case HandReticle.TextType.Hand:
                    instance.textHand += text;
                    return;
                case HandReticle.TextType.HandSubscript:
                    instance.textHandSubscript += text;
                    return;
                case HandReticle.TextType.Use:
                    instance.textUse += text;
                    return;
                case HandReticle.TextType.UseSubscript:
                    instance.textUseSubscript += text;
                    return;
                default:
                    return;
            }
        }
    }
}