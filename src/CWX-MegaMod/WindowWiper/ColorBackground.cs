// using System.Reflection;
// using EFT.UI.DragAndDrop;
// using HarmonyLib;
// using SPT.Reflection.Patching;
// using UnityEngine;
//
// namespace CWX_MegaMod.WindowWiper
// {
//     public class ColorBackground : ModulePatch
//     {
//         protected override MethodBase GetTargetMethod()
//         {
//             return AccessTools.Method(typeof(GridItemView), nameof(GridItemView.UpdateInfo));
//         }
//
//         [PatchPostfix]
//         public static void PatchPostfix(ref GridItemView __instance, ref Color ___BackgroundColor, bool ___bool_5)
//         {
//             if (!__instance.Examined)
//             {
//                 return;
//             }
//
//             if (__instance.method_32(__instance.Item) || ___bool_5 || __instance.IsConflicting)
//             {
//                 return;
//             }
//
//             // do your logic
//             ___BackgroundColor = Color.blue;
//             __instance.UpdateColor();
//         }
//     }
// }