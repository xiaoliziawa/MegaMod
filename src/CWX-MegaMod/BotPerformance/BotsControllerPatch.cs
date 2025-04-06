using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using UnityEngine;

namespace CWX_MegaMod.BotPerformance
{
    public class BotsControllerPatch : ModulePatch
    {
        private static bool PatchInUse = false;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(BotsController), nameof(BotsController.method_0));
        }

        [PatchPrefix]
        public static bool PatchPrefix(BotsController __instance)
        {
            // MegaMod.Logger.LogWarning($"BotsControllerPatch start");
            if (PatchInUse)
            {
                // MegaMod.Logger.LogWarning($"BotsControllerPatch in use, returning");
                return MegaMod.DontRunOriginal;
            }

            PatchInUse = true;
            UnitaskTest(__instance);
            return MegaMod.DontRunOriginal;
        }

        private static async UniTaskVoid UnitaskTest(object __instance)
        {
            // MegaMod.Logger.LogWarning($"ManualUpdateReplacement start");
            var controller = __instance as BotsController;
            controller.ArtilleryZonesController.ManualUpdate();
            controller.AICoreController.Update();
            controller.AiTaskManager.Update();
            controller.Bots.UpdateByUnity();
            controller.EventsController.ManualUpdate();
            PatchInUse = false;
            // MegaMod.Logger.LogWarning($"ManualUpdateReplacement end");
        }

        private static void ManualUpdateReplacement(object __instance)
        {
            // MegaMod.Logger.LogWarning($"ManualUpdateReplacement start");
            var controller = __instance as BotsController;
            controller.ArtilleryZonesController.ManualUpdate();
            controller.AICoreController.Update();
            controller.AiTaskManager.Update();
            controller.Bots.UpdateByUnity();
            controller.EventsController.ManualUpdate();
            PatchInUse = false;
            // MegaMod.Logger.LogWarning($"ManualUpdateReplacement end");
        }
    }
}