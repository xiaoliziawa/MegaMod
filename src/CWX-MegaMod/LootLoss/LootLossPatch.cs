using System.Collections.Generic;
using System.Reflection;
using EFT;
using HarmonyLib;
using JsonType;
using SPT.Reflection.Patching;

namespace CWX_MegaMod.LootLoss
{
    public class LootLossPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(LocalGame), nameof(LocalGame.Create));
        }

        [PatchPrefix]
        public static void PatchPrefix(ref LocalRaidSettings raidSettings)
        {
            if (!MegaMod.LootLoss.Value)
            {
                return;
            }
            
            var location = raidSettings.selectedLocation;
            location.containers = new Dictionary<string, LocationSettings.Location.LootContainer>();
            location.Loot = new LootData();
        }
    }
}
