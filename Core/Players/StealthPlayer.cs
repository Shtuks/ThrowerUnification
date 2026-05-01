using CalamityMod;
using Terraria.ModLoader;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Core.Players
{
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class StealthPlayer : ModPlayer
    {
        private static readonly Mod thorium = ModCompatibility.Thorium.Mod;
        private static readonly Mod helhiem = ModCompatibility.ThoriumRework.Mod;
        private static readonly Mod consolaria = ModCompatibility.Consolaria.Mod;

        public override void PostUpdateEquips()
        {
            if (ModCompatibility.Thorium.Loaded)
            {
                if (ModCompatibility.ThoriumRework.Loaded)
                {
                    if (helhiem.TryFind("TitanHood", out ModItem titanHood))
                    {
                        if (CheckArmorSet(titanHood.Type, thorium.Find<ModItem>("TitanBreastplate").Type, thorium.Find<ModItem>("TitanGreaves").Type))
                        {
                            AddStealth(110);
                        }
                    }
                }

                if (ModCompatibility.Consolaria.Loaded)
                {
                    int viperHelmet = consolaria.Find<ModItem>("ViperHelmet").Type;
                    int viperHelmentOld = consolaria.Find<ModItem>("OldViperHelmet").Type;

                    if (CheckArmorSet(viperHelmet, consolaria.Find<ModItem>("ViperChestplate").Type, consolaria.Find<ModItem>("ViperLegs").Type) || CheckArmorSet(viperHelmentOld, consolaria.Find<ModItem>("ViperChestplate").Type, consolaria.Find<ModItem>("ViperLegs").Type) ||
                        CheckArmorSet(viperHelmet, consolaria.Find<ModItem>("OldViperChestplate").Type, consolaria.Find<ModItem>("OldViperLegs").Type) || CheckArmorSet(viperHelmentOld, consolaria.Find<ModItem>("OldViperChestplate").Type, consolaria.Find<ModItem>("OldViperLegs").Type))
                    {
                        AddStealth(110);
                    }
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            float currentStealth = ModCompatibility.Calamity.Mod.Call("GetCurrentStealth", Player) is float f ? f : 1f;
            bool stealthStrike = currentStealth > 0f && (Player.HeldItem.CountsAsClass<RogueDamageClass>() || Player.HeldItem.CountsAsClass<ThrowingDamageClass>() || Player.HeldItem.CountsAsClass<UnitedModdedThrower>());

            if (stealthStrike)
            {
                Player.GetAttackSpeed(DamageClass.Generic) = 1f;
                Player.GetAttackSpeed(DamageClass.Throwing) = 1f;
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
