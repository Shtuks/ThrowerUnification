using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Core.Players
{
    //Akira & Wardrobe Hummus
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class CalamityThrowerPlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            Player player = Main.LocalPlayer;
            var CalPlayer = player.GetModPlayer<CalamityPlayer>();

            Player.GetDamage<UnitedModdedThrower>() += CalPlayer.stealthDamage;
        }

        // Provides a damage boost to non-calamity weapons when a stealth strike is available.
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (!ThrowerModConfig.Instance.StealthStrikes) return;

            bool isModded = item.ModItem != null;
            string modName = isModded ? item.ModItem.Mod.Name : null;

            bool isMergedRogue = item.DamageType?.ToString() == "CalamityMod.RogueDamageClass"
                                 || item.DamageType == UnitedModdedThrower.Instance;

            bool isNotCalamityAndConsumableRogue = isModded && modName != "CalamityMod"
                                                   && item.consumable && isMergedRogue;

            bool isFromAllowedMod = isModded && isMergedRogue;

            bool isThoriumNonConsumable = isModded && modName == "ThoriumMod" && !item.consumable;

            // Vanilla items (no ModItem) that use rogue damage
            bool isVanillaRogue = !isModded && isMergedRogue;

            if (isNotCalamityAndConsumableRogue || isFromAllowedMod || isVanillaRogue)
            {
                if (isThoriumNonConsumable)
                {
                    return;
                }

                Player player = Main.LocalPlayer;
                var calPlayer = player.GetModPlayer<CalamityPlayer>();

                if (calPlayer.StealthStrikeAvailable())
                {
                    string[] preventDamageBoostItems =
                    {
                        "Clockwork Bomb",
                        "Soul Bomb",
                        "Soulslasher",
                        "Soft Serve Sunderer",
                        "Shade Shuriken",
                        "Dracula Fang",
                        "Wack Wrench"
                    };

                    for (int i = 0; i < preventDamageBoostItems.Length; i++)
                    {
                        if (item.Name == preventDamageBoostItems[i])
                            return;
                    }

                    if (item.consumable || isFromAllowedMod)
                        damage *= 1.75f;
                }
            }
        }
    }
}
