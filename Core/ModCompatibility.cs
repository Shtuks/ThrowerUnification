using Terraria.ModLoader;

namespace ThrowerUnification.Core;

public static class ModCompatibility
{
    public static class Thorium
    {
        public const string Name = "ThoriumMod";
        public static bool Loaded => ModLoader.HasMod(Name);
        public static Mod Mod => ModLoader.GetMod(Name);
    }  
    public static class Calamity
    {
        public const string Name = "CalamityMod";
        public static bool Loaded => ModLoader.HasMod(Name);
        public static Mod Mod => ModLoader.GetMod(Name);
    }
    public static class SacredTools
    {
        public const string Name = "SacredTools";
        public static bool Loaded => ModLoader.HasMod(Name);
        public static Mod Mod => ModLoader.GetMod(Name);
    }
    public static class Vitality
    {
        public const string Name = "VitalityMod";
        public static bool Loaded => ModLoader.HasMod(Name);
        public static Mod Mod => ModLoader.GetMod(Name);
    }
    public static class SOTSBardThrowerHealer
    {
        public const string Name = "SOTSBardHealer";
        public static bool Loaded => ModLoader.HasMod(Name);
        public static Mod Mod => ModLoader.GetMod(Name);
    }

    //OTHER MAJOR UMT MERGE MODS
    public static class CSE
    {
        public const string Name = "ssm";
        public static bool Loaded => ModLoader.HasMod(Name);
        public static Mod Mod => ModLoader.GetMod(Name);
    }
    public static class InfernalEclipse
    {
        public const string Name = "InfernalEclipseAPI";
        public static bool Loaded => ModLoader.HasMod(Name);
        public static Mod Mod => ModLoader.GetMod(Name);
    }
    public static class WHummusMultiModBalancing
    {
        public const string Name = "WHummusMultiModBalancing";
        public static bool Loaded => ModLoader.HasMod(Name);
        public static Mod Mod => ModLoader.GetMod(Name);
    }
}
