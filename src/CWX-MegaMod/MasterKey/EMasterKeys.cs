using System.ComponentModel;

namespace CWX_MegaMod.Config
{
    public enum EMasterKeys
    {
        [Description("实验室黄色钥匙卡")]
        Yellow,
        [Description("实验室绿色钥匙卡")]
        Green,
        [Description("实验室蓝色钥匙卡")]
        Blue,
        [Description("实验室红色钥匙卡")]
        Red,
        [Description("实验室紫色钥匙卡")]
        Violet,
        [Description("实验室黑色钥匙卡")]
        Black,
        [Description("实验室门禁钥匙卡")]
        Access,
        [Description("实验室储藏室钥匙")]
        Storage,
        [Description("住宅单元钥匙")]
        Residential,
        [Description("11SR 号物件钥匙卡")]
        ElevenSR,
        [Description("21WS 号物件钥匙卡")]
        TwentyOneWS,
        [Description("带蓝色标记的钥匙卡")]
        BlueMarked
    }
}
