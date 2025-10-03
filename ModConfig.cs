using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ThrowerUnification
{
    [BackgroundColor(32, 50, 32, 216)]
    public class ThrowerModConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static ThrowerModConfig Instance;

        [Header("General")]

        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(true)]
        public bool Vanilla { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(true)]
        public bool Calamity { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(true)]
        public bool Vitality { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(true)]
        public bool SacredTools { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(false)]
        public bool ThoriumClassTag { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(true)]
        public bool StealthStrikes { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DrawTicks]
        [DefaultValue(TooltipOverrideStyle.Thrower)]
        public TooltipOverrideStyle TooltipOverride { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue("")]
        public string CustomTooltipOverride { get; set; }

        [Header("ColoredDamageType")]

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(255)]
        public int R { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(100)]
        public int G { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(100)]
        public int B { get; set; }
    }
    public enum TooltipOverrideStyle : byte
    {
        Thrower,
        Rogue,
        Malevolent,
        Kinetic,
        Custom
    }
}
