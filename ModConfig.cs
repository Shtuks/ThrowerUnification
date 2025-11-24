using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [DefaultValue(true)]
        public bool StealthStrikes { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(true)]
        public bool LegacyVanillaThrowerWeapons { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DrawTicks]
        [DefaultValue(TooltipOverrideStyle.Thrower)]
        public TooltipOverrideStyle TooltipOverride { get; set; }

        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue("")]
        public string CustomTooltipOverride { get; set; }

        [Header("ClassTag")]

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(false)]
        public bool ThoriumClassTag { get; set; }

        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(255)]
        public byte TagR { get; set; }

        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(165)]
        public byte TagG { get; set; }

        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(60)]
        public byte TagB { get; set; }

        [Header("ColoredDamageType")]

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(255)]
        public byte R { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(100)]
        public byte G { get; set; }

        [ReloadRequired]
        [BackgroundColor(60, 200, 60, 192)]
        [DefaultValue(100)]
        public byte B { get; set; }
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
