using System.Reflection;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace CWX_MegaMod.PainkillerDesat
{
    public class PainkillerDesatScript2 : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(EffectsController.DesaturateMaskAccumulator),
                nameof(EffectsController.DesaturateMaskAccumulator.UpdateDesaturate));
        }

        [PatchPrefix] // removes the sharpen/desat effect from some painkillers
        public static bool PatchPrefix(ref CC_Sharpen ___cc_Sharpen_0)
        {
            if (!MegaMod.PainkillerDesat.Value)
            {
                return true;
            }

            PainkillerDesatHelper.DisableSharpenEffects(___cc_Sharpen_0);

            return false; // dont do method
        }
    }
}
