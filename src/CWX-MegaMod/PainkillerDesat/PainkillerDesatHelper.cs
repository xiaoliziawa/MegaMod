namespace CWX_MegaMod.PainkillerDesat
{
    internal static class PainkillerDesatHelper
    {
        private const float DisabledMaskDesaturation = 0f;
        private const float DefaultRadius = 1f;
        private const float DefaultRadiusFalloff = 0.425f;

        public static void DisableSharpenEffects(CC_Sharpen sharpen)
        {
            if (sharpen == null)
            {
                return;
            }

            sharpen.MaskDesaturate = DisabledMaskDesaturation;
            sharpen.Radius = DefaultRadius;
            sharpen.RadiusFalloff = DefaultRadiusFalloff;

            var settingsProvider = sharpen.DesaturateEffectSettingsProvider;
            if (settingsProvider == null)
            {
                return;
            }

            settingsProvider.MaskDesaturate = DisabledMaskDesaturation;
            settingsProvider.Radius = DefaultRadius;
            settingsProvider.RadiusFalloff = DefaultRadiusFalloff;
        }
    }
}
