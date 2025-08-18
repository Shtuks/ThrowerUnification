using Terraria.ModLoader;

namespace ThrowerUnification;

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
    public static class Ragnarok
    {
        public const string Name = "RagnarokMod";
        public static bool Loaded => ModLoader.HasMod(Name);
        public static Mod Mod => ModLoader.GetMod(Name);
    }
    public static class SOTSBardThrowerHealer
    {
        public const string Name = "SOTSBardHealer";
        public static bool Loaded => ModLoader.HasMod(Name);
        public static Mod Mod => ModLoader.GetMod(Name);
    }
}
