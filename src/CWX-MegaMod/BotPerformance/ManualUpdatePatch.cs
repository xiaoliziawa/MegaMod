using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EFT;
using EFT.Game.Spawning;
using HarmonyLib;
using SPT.Reflection.Patching;
using UnityEngine;
using UnityEngine.AI;

namespace CWX_MegaMod.BotPerformance
{
    public class ManualUpdatePatch : ModulePatch
    {
        private static ConcurrentDictionary<int, BotUsageClass> UpdateDictionary = new ConcurrentDictionary<int, BotUsageClass>();
        private static FieldInfo NextGetGoalField = typeof(BotOwner).GetField("_nextGetGoalTime", AccessTools.all);
        private static FieldInfo NextTimeCheckBorn = typeof(BotOwner).GetField("_nextTimeCheckBorn", AccessTools.all);

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(BotOwner), nameof(BotOwner.UpdateManual));
        }

        [PatchPrefix]
        public static bool PatchPrefix(ref BotOwner __instance) // want the instance,
        {
            // MegaMod.Logger.LogWarning("Start Patch");
            if (__instance.BotState != EBotState.Active || !__instance.HealthController.IsAlive)
            {
                if (UpdateDictionary.TryGetValue(__instance.Id, out BotUsageClass bot))
                {
                    UpdateDictionary.TryRemove(__instance.Id, out _);
                }
                // MegaMod.Logger.LogWarning($"bot {__instance.Id} was either dead or not active yet");
                return MegaMod.RunOriginal;
            }

            if (!UpdateDictionary.TryGetValue(__instance.Id, out BotUsageClass botUsageClass))
            {
                var botUsage = new BotUsageClass
                {
                    BotOwner = __instance,
                    IsInUse = false
                };

                UpdateDictionary.TryAdd(__instance.Id, botUsage);
                // MegaMod.Logger.LogWarning($"added {__instance.Id} to dictionary");
            }

            if (botUsageClass.IsInUse)
            {
                // MegaMod.Logger.LogWarning($"bot {__instance.Id} in use");
                return MegaMod.DontRunOriginal;
            }

            if (!botUsageClass.IsInUse)
            {
                // MegaMod.Logger.LogWarning($"bot {__instance.Id} not in use");
                botUsageClass.IsInUse = true;

                // ThreadPool.QueueUserWorkItem(ManualUpdateReplacement, __instance);
                // Task.Factory.StartNew(ManualUpdateReplacement, __instance);
                // ManualUpdateReplacement(__instance);
                UnitaskTest(__instance);
                return MegaMod.DontRunOriginal;
            }

            // MegaMod.Logger.LogError("something went wrong");
            return MegaMod.RunOriginal;
        }

        private static async UniTaskVoid UnitaskTest(object state)
        {
            var botOwner = (BotOwner)state;
            // MegaMod.Logger.LogWarning($"ManualUpdateReplacement {botOwner.Id} start");

            if (botOwner.BotState == EBotState.Active && botOwner.GetPlayer.HealthController.IsAlive)
            {
                botOwner.StandBy.Update();
                botOwner.LookSensor.ManualUpdate();
                if (botOwner.StandBy.StandByType != BotStandByType.paused)
                {
                    if ((NextGetGoalField.GetValue(botOwner) as float?) < Time.time)
                    {
                        botOwner.CalcGoal();
                    }
                    botOwner.SuppressShoot.ManualUpdate();
                    botOwner.HeadData.ManualUpdate();
                    botOwner.ShootData.ManualUpdate();
                    botOwner.Tilt.ManualUpdate();
                    botOwner.NightVision.ManualUpdate();
                    botOwner.NearDoorData.Update();
                    botOwner.DogFight.ManualUpdate();
                    botOwner.FriendChecker.ManualUpdate();
                    botOwner.RecoilData.LosingRecoil();
                    botOwner.Mover.ManualUpdate();
                    botOwner.AimingManager.ManualUpdate();
                    botOwner.Medecine.ManualUpdate();
                    botOwner.Boss.ManualUpdate();
                    botOwner.BotTalk.ManualUpdate();
                    botOwner.WeaponManager.ManualUpdate();
                    botOwner.BotRequestController.Update();
                    botOwner.GrenadeToPortal.ManualUpdate();
                    botOwner.Tactic.UpdateChangeTactics();
                    botOwner.Memory.ManualUpdate(Time.deltaTime);
                    botOwner.Settings.UpdateManual();
                    botOwner.BotRequestController.TryToFind();
                    botOwner.WarnData.ManualUpdate();
                    botOwner.ArtilleryDangerPlace.ManualUpdate();
                    if (botOwner.GetPlayer.UpdateQueue == EUpdateQueue.Update)
                    {
                        botOwner.Mover.ManualFixedUpdate();
                        botOwner.Steering.ManualFixedUpdate();
                    }
                }

                // MegaMod.Logger.LogWarning($"ManualUpdateReplacement {botOwner.Id} end");
                if (UpdateDictionary.TryGetValue(botOwner.Id, out BotUsageClass botUsageClass))
                {
                    botUsageClass.IsInUse = false;
                }

                return;
            }

            if (botOwner.BotState == EBotState.PreActive && botOwner.WeaponManager.IsReady)
            {
                // MegaMod.Logger.LogWarning($"ManualUpdateReplacement {botOwner.Id} PreActive");
                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(botOwner.GetPlayer.Position, out navMeshHit, 0.6f, -1))
                {
                    botOwner.method_10();
                    return;
                }
                if ((NextTimeCheckBorn.GetValue(botOwner) as float?) < Time.time)
                {
                    NextTimeCheckBorn.SetValue(botOwner, Time.time + 1f);
                    botOwner.Transform.position = botOwner.BotsGroup.BotZone.SpawnPoints.RandomElement<ISpawnPoint>().Position + Vector3.up * 0.5f;
                    botOwner.method_10();
                }
            }
        }

        private static void ManualUpdateReplacement(object state)
        {
            var botOwner = (BotOwner)state;
            MegaMod.Logger.LogWarning($"ManualUpdateReplacement {botOwner.Id} start");

            if (botOwner.BotState == EBotState.Active && botOwner.GetPlayer.HealthController.IsAlive)
            {
                botOwner.StandBy.Update();
                botOwner.LookSensor.ManualUpdate();
                if (botOwner.StandBy.StandByType != BotStandByType.paused)
                {
                    if ((NextGetGoalField.GetValue(botOwner) as float?) < Time.time)
                    {
                        botOwner.CalcGoal();
                    }
                    botOwner.SuppressShoot.ManualUpdate();
                    botOwner.HeadData.ManualUpdate();
                    botOwner.ShootData.ManualUpdate();
                    botOwner.Tilt.ManualUpdate();
                    botOwner.NightVision.ManualUpdate();
                    botOwner.NearDoorData.Update();
                    botOwner.DogFight.ManualUpdate();
                    botOwner.FriendChecker.ManualUpdate();
                    botOwner.RecoilData.LosingRecoil();
                    botOwner.Mover.ManualUpdate();
                    botOwner.AimingManager.ManualUpdate();
                    botOwner.Medecine.ManualUpdate();
                    botOwner.Boss.ManualUpdate();
                    botOwner.BotTalk.ManualUpdate();
                    botOwner.WeaponManager.ManualUpdate();
                    botOwner.BotRequestController.Update();
                    botOwner.GrenadeToPortal.ManualUpdate();
                    botOwner.Tactic.UpdateChangeTactics();
                    botOwner.Memory.ManualUpdate(Time.deltaTime);
                    botOwner.Settings.UpdateManual();
                    botOwner.BotRequestController.TryToFind();
                    botOwner.WarnData.ManualUpdate();
                    botOwner.ArtilleryDangerPlace.ManualUpdate();
                    if (botOwner.GetPlayer.UpdateQueue == EUpdateQueue.Update)
                    {
                        botOwner.Mover.ManualFixedUpdate();
                        botOwner.Steering.ManualFixedUpdate();
                    }
                }

                MegaMod.Logger.LogWarning($"ManualUpdateReplacement {botOwner.Id} end");
                if (UpdateDictionary.TryGetValue(botOwner.Id, out BotUsageClass botUsageClass))
                {
                    botUsageClass.IsInUse = false;
                }

                return;
            }

            if (botOwner.BotState == EBotState.PreActive && botOwner.WeaponManager.IsReady)
            {
                MegaMod.Logger.LogWarning($"ManualUpdateReplacement {botOwner.Id} PreActive");
                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(botOwner.GetPlayer.Position, out navMeshHit, 0.6f, -1))
                {
                    botOwner.method_10();
                    return;
                }
                if ((NextTimeCheckBorn.GetValue(botOwner) as float?) < Time.time)
                {
                    NextTimeCheckBorn.SetValue(botOwner, Time.time + 1f);
                    botOwner.Transform.position = botOwner.BotsGroup.BotZone.SpawnPoints.RandomElement<ISpawnPoint>().Position + Vector3.up * 0.5f;
                    botOwner.method_10();
                }
            }
        }
    }

    public class BotUsageClass
    {
        public BotOwner BotOwner { get; set; }
        public bool IsInUse { get; set; } = false;
    }
}