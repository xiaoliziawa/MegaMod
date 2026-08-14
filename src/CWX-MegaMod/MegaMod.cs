using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CWX_MegaMod.ChadMode;
using CWX_MegaMod.PainkillerDesat;
using CWX_MegaMod.Config;
using CWX_MegaMod.InventoryViewer;
using CWX_MegaMod.LootLoss;
using CWX_MegaMod.SpaceUser;
using CWX_MegaMod.TradingPlayerView;
using EFT.Communications;
using EFT.UI;
using UnityEngine;

namespace CWX_MegaMod
{
    [BepInPlugin("com.cwx.megamod", "CWX-MegaMod", "4.0.1")]
    public class MegaMod : BaseUnityPlugin
    {
        private const string GeneralCategory = "1- 常规功能";
        private const string DebugCategory = "2- 调试功能";
        private const string MasterKeyCategory = "3- 万能钥匙设置";

        internal new static ManualLogSource Logger { get; private set; }
        internal static ConfigEntry<bool> BushWhacker { get; private set; }
        internal static ConfigEntry<bool> GrassCutter { get; private set; }
        internal static ConfigEntry<bool> MasterKey { get; private set; }
        internal static ConfigEntry<EMasterKeys> MasterKeyToUse { get; private set; }
        internal static ConfigEntry<bool> TradingPlayerView { get; private set; }
        internal static ConfigEntry<bool> SpaceUser { get; private set; }
        internal static ConfigEntry<bool> EnvironmentEnjoyer { get; private set; }
        internal static ConfigEntry<bool> InventoryViewer { get; private set; }
        internal static ConfigEntry<bool> PainkillerDesat { get; private set; }
        internal static ConfigEntry<bool> ReserveAlarmChanger { get; private set; }
        internal static ConfigEntry<bool> GodMode { get; private set; }
        internal static ConfigEntry<bool> ThermalMode { get; private set; }
        internal static ConfigEntry<bool> BetterThermalMode { get; private set; }
        internal static ConfigEntry<bool> NightVisionMode { get; private set; }
        internal static ConfigEntry<bool> CameraShake { get; private set; }
        internal static ConfigEntry<bool> LootLoss { get; private set; }
        internal static ConfigEntry<bool> FoodWater { get; private set; }
        internal static ConfigEntry<bool> InstantSearch { get; private set; }
        // internal static ConfigEntry<bool> NewContent { get; private set; }

        public void Awake()
        {
            Logger = base.Logger;
            InitConfig();

            new MegaModPatch().Enable();
            new SpaceUserSplitPatch().Enable();
            new SpaceUserFleaPatch().Enable();
            new TradingPlayerItemViewPatch().Enable();
            new InventoryViewerPatch().Enable();
            new PainkillerDesatScript1().Enable();
            new PainkillerDesatScript2().Enable();
            new PainkillerDesatScript3().Enable();
            new PainkillerDesatScript4().Enable();
            new CameraShakePatch().Enable();
            new LootLossPatch().Enable();
            new HydrationPatch().Enable();
            new EnergyPatch().Enable();
        }

        // Higher order number comes first
        private void InitConfig()
        {
            // Normal mods
            ReserveAlarmChanger = Config.Bind("1- All Mods", "ReserveAlarmChanger - On/Off", false,
                CreateChineseConfigDescription(GeneralCategory, "储备站警报音效替换",
                    "使用插件 Sounds 文件夹内的音频替换储备站警报声，仅在战局开始时加载。", 11));
            BushWhacker = Config.Bind("1- All Mods", "BushWhacker - On/Off", false,
                CreateChineseConfigDescription(GeneralCategory, "移除灌木减速",
                    "移除角色穿过灌木时的减速效果。", 10));
            GrassCutter = Config.Bind("1- All Mods", "GrassCutter - On/Off", false,
                CreateChineseConfigDescription(GeneralCategory, "隐藏草地",
                    "隐藏地图中的草。", 9));
            MasterKey = Config.Bind("1- All Mods", "MasterKey - On/Off", false,
                CreateChineseConfigDescription(GeneralCategory, "万能钥匙",
                    "让所有可解锁的门使用“万能钥匙类型”中选择的钥匙。", 8));
            EnvironmentEnjoyer = Config.Bind("1- All Mods", "EnvironmentEnjoyer - On/Off", false,
                CreateChineseConfigDescription(GeneralCategory, "隐藏树木和灌木",
                    "隐藏地图中的树木和灌木。", 7));
            SpaceUser = Config.Bind("1- All Mods", "SpaceUser - On/Off", false,
                CreateChineseConfigDescription(GeneralCategory, "空格键确认",
                    "允许使用空格键确认跳蚤市场操作和物品堆叠拆分。", 6));
            TradingPlayerView = Config.Bind("1- All Mods", "TradingPlayerView - On/Off", false,
                CreateChineseConfigDescription(GeneralCategory, "商人物品视图",
                    "调整商人交易界面中的玩家物品显示。", 5));
            PainkillerDesat = Config.Bind("1- All Mods", "PainkillerDesat - On/Off", false,
                CreateChineseConfigDescription(GeneralCategory, "移除止痛药视觉效果",
                    "移除服用止痛药后产生的视觉效果。", 3));
            // NewContent = Config.Bind("1- All Mods", "NewContent - On/Off", false,
            //     new ConfigDescription("Enable NewContent",
            //         tags: new ConfigurationManagerAttributes() { Order = 1 }));

            // MasterKey Settings
            MasterKeyToUse = Config.Bind("3- MasterKey", "MasterKeyToUse", EMasterKeys.Yellow,
                CreateChineseConfigDescription(MasterKeyCategory, "万能钥匙类型",
                    "启用万能钥匙后，所有可解锁的门都将使用这里选择的钥匙。", 1));

            // Debugging Mods
            InstantSearch = Config.Bind("2- Debug Mods", "InstantSearch - On/Off", false,
                CreateChineseConfigDescription(DebugCategory, "即时搜索",
                    "无需等待即可完成物品搜索。", 10));
            FoodWater = Config.Bind("2- Debug Mods", "FoodWater - On/Off", false,
                CreateChineseConfigDescription(DebugCategory, "无限水分与能量",
                    "停止消耗角色的水分和能量。", 9));
            LootLoss = Config.Bind("2- Debug Mods", "LootLoss - On/Off", false,
                CreateChineseConfigDescription(DebugCategory, "移除地图战利品",
                    "在地图加载时移除全部战利品。", 8));
            InventoryViewer = Config.Bind("2- Debug Mods", "InventoryViewer - On/Off", false,
                CreateChineseConfigDescription(DebugCategory, "完整库存查看",
                    "允许查看容器中的全部物品栏内容。", 6));
            GodMode = Config.Bind("2- Debug Mods", "GodMode - On/Off", false,
                CreateChineseConfigDescription(DebugCategory, "无敌模式",
                    "使角色不会死亡。", 5));
            CameraShake = Config.Bind("2- Debug Mods", "CameraShake - On/Off", false,
                CreateChineseConfigDescription(DebugCategory, "移除镜头抖动",
                    "移除角色受伤时的镜头抖动。", 4));
            ThermalMode = Config.Bind("2- Debug Mods", "ThermalMode - On/Off", false,
                CreateChineseConfigDescription(DebugCategory, "热成像模式",
                    "启用热成像画面。", 3));
            BetterThermalMode = Config.Bind("2- Debug Mods", "BetterThermalMode - On/Off", false,
                CreateChineseConfigDescription(DebugCategory, "增强热成像",
                    "移除热成像画面的模糊、噪点等效果。", 2));
            NightVisionMode = Config.Bind("2- Debug Mods", "NightVisionMode - On/Off", false,
                CreateChineseConfigDescription(DebugCategory, "夜视模式",
                    "启用夜视画面。", 1));
        }

        private static ConfigDescription CreateChineseConfigDescription(
            string category, string displayName, string description, int order)
        {
            return new ConfigDescription(description,
                tags: new ConfigurationManagerAttributes
                {
                    Category = category,
                    DispName = displayName,
                    Description = description,
                    Order = order
                });
        }

        public static void LogToScreen(string message = "", EMessageType eMessageType = EMessageType.Info)
        {
            switch (eMessageType)
            {
                case EMessageType.NotiError:
                    ConsoleScreen.LogError("[CWX-MegaMod Error] " + message);
                    Logger.LogError("[CWX-MegaMod Error] " + message);
                    NotificationManager.DisplayMessageNotification("[CWX-MegaMod Error] " + message,
                        ENotificationDurationType.Default, ENotificationIconType.Alert, Color.red);
                    break;
                case EMessageType.NotiWarn:
                    ConsoleScreen.LogWarning("[CWX-MegaMod Warning] " + message);
                    Logger.LogWarning("[CWX-MegaMod Warning] " + message);
                    NotificationManager.DisplayMessageNotification("[CWX-MegaMod Warning] " + message,
                        ENotificationDurationType.Default, ENotificationIconType.Default, Color.yellow);
                    break;
                case EMessageType.NotiInfo:
                    ConsoleScreen.Log("[CWX-MegaMod Info] " + message);
                    Logger.LogDebug("[CWX-MegaMod Info] " + message);
                    NotificationManager.DisplayMessageNotification("[CWX-MegaMod Info] " + message,
                        ENotificationDurationType.Default, ENotificationIconType.Friend, Color.cyan);
                    break;
                case EMessageType.Error:
                    ConsoleScreen.LogError("[CWX-MegaMod Error] " + message);
                    Logger.LogError("[CWX-MegaMod Error] " + message);
                    break;
                case EMessageType.Warning:
                    ConsoleScreen.LogWarning("[CWX-MegaMod Warning] " + message);
                    Logger.LogWarning("[CWX-MegaMod Warning] " + message);
                    break;
                case EMessageType.Info:
                default:
                    ConsoleScreen.Log("[CWX-MegaMod Info] " + message);
                    Logger.LogDebug("[CWX-MegaMod Info] " + message);
                    break;
            }
        }
    }

    public enum EMessageType
    {
        NotiError,
        NotiWarn,
        NotiInfo,
        Error,
        Warning,
        Info
    }
}
