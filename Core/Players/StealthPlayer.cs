using CalamityMod;
using Terraria.ModLoader;

namespace ThrowerUnification.Core.Players
{
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class StealthPlayer : ModPlayer
    {
        private static readonly Mod thorium = ModCompatibility.Thorium.Mod;
        private static readonly Mod helhiem = ModCompatibility.ThoriumRework.Mod;

        public override void PostUpdateEquips()
        {
            if (ModCompatibility.ThoriumRework.Loaded) 
            {
                if (CheckArmorSet(helhiem.Find<ModItem>("TitanHood").Type, thorium.Find<ModItem>("TitanBreastplate").Type, thorium.Find<ModItem>("TitanGreaves").Type))
                {
                    AddStealth(110);
                }
                
            }
        }

        public void AddStealth(int stealth)
        {
            Player.Calamity().rogueStealthMax += stealth / 100f;
            Player.Calamity().wearingRogueArmor = true;
            Player.setBonus = $"{Player.setBonus}\n+{stealth.ToString()} maximum stealth";
        }

        public bool CheckArmorSet(int head, int body, int legs)
        {
            if (Player.armor[0].type == head && Player.armor[1].type == body && Player.armor[2].type == legs)
                return true;
            return false;
        }
    }
}
