using System.Reflection;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace CWX_MegaMod.PainkillerDesat
{
    public class PainkillerDesatScript4 : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(EffectsController.CC_DoubleVisionAccumulator),
                nameof(EffectsController.CC_DoubleVisionAccumulator.Toggle));
        }

        [PatchPrefix] // removes the double vision effect from some painkillers
        public static bool PatchPrefix(ref CC_DoubleVision ___cc_DoubleVision_0)
        {
            if (!MegaMod.PainkillerDesat.Value)
            {
                return true;
            }

            if (___cc_DoubleVision_0 != null)
            {
                ___cc_DoubleVision_0.enabled = false;
            }

            return false; // dont do method
        }
    }
}
