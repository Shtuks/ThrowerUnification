using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria;
using Terraria.ID;
using System.Reflection;
using Microsoft.Xna.Framework;
using CalamityMod.CalPlayer;
using CalamityMod;
using Terraria.Localization;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Content.StealthStrikes
{
    public class DefaultStealthStrike : ModPlayer
    {

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            // List of allowed mod names
            var allowedMods = new HashSet<string>
            {
                "BCThrower",
                "ThrowerPostGame",
                "Arsenal_Mod",
                "ThrowerArsenalAddOn",
                "SacredTools",
                "VitalityMod"
            };

            bool isModded = item.ModItem != null;
            string modName = isModded ? item.ModItem.Mod.Name : null;

            bool isMergedRogue = item.DamageType?.ToString() == "CalamityMod.RogueDamageClass"
                                 || item.DamageType == UnitedModdedThrower.Instance;

            bool isNotCalamityAndConsumableRogue = isModded && modName != "CalamityMod"
                                                   && item.consumable && isMergedRogue;

            bool isFromAllowedMod = isModded && allowedMods.Contains(modName) && isMergedRogue;

            // Vanilla items (no ModItem) that use rogue damage
            bool isVanillaRogue = !isModded && isMergedRogue;

            if (isNotCalamityAndConsumableRogue || isFromAllowedMod || isVanillaRogue)
            {
                Player player = Main.LocalPlayer;
                var calPlayer = player.GetModPlayer<CalamityPlayer>();

                if (calPlayer.StealthStrikeAvailable())
                {
                    if (item.Name == "Clockwork Bomb" || item.Name == "Soul Bomb" || item.Name == "Soulslasher"
                        || item.Name == "Soft Serve Sunderer" || item.Name == "Shade Shuriken")
                        return;

                    if (item.consumable || isFromAllowedMod)
                        damage *= 1.75f;
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            Player player = Main.LocalPlayer;
            var CalPlayer = player.GetModPlayer<CalamityPlayer>();
            Player.GetDamage<UnitedModdedThrower>() += CalPlayer.stealthDamage;
        }
    }

    public class WhummusGlobalItem : GlobalItem
    {
        // STEALTH PROJECTILE SPEED STUFF
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.Name == "Captain's Poignard")
                return;

            // Define allowed mod names
            var allowedMods = new HashSet<string>
            {
                "BCThrower",
                "ThrowerPostGame",
                "Arsenal_Mod",
                "ThrowerArsenalAddOn",
                "SacredTools",
                "VitalityMod"
            };

            bool isModded = item.ModItem != null;
            string modName = isModded ? item.ModItem.Mod.Name : null;

            bool isMergedRogue = item.DamageType?.ToString() == "CalamityMod.RogueDamageClass"
                                 || item.DamageType == UnitedModdedThrower.Instance;

            bool isNotCalamityAndConsumableRogue = isModded && modName != "CalamityMod"
                                                   && item.consumable && isMergedRogue;

            bool isFromAllowedMod = isModded && allowedMods.Contains(modName) && isMergedRogue;

            // Vanilla items (no ModItem) that use rogue damage
            bool isVanillaRogue = !isModded && isMergedRogue;

            if (isNotCalamityAndConsumableRogue || isFromAllowedMod || isVanillaRogue)
            {
                var CalPlayer = player.GetModPlayer<CalamityPlayer>();
                if (CalPlayer.StealthStrikeAvailable())
                {
                    velocity *= 1.75f;
                }
            }
        }


        private static int[] supportedTypes;
        private static bool initialized = false;

        private void EnsureInitialized()
        {
            if (initialized) return;

            List<int> types = new();

            if (ModContent.TryFind<ModItem>("ThoriumMod/ThrowingGuideVolume2", out var modItem2))
                types.Add(modItem2.Type);
            if (ModContent.TryFind<ModItem>("ThoriumMod/ThrowingGuideVolume3", out var modItem3))
                types.Add(modItem3.Type);
            if (ModContent.TryFind<ModItem>("ssm/GtTETFinal", out var modItem4))
                types.Add(modItem4.Type);

            supportedTypes = types.ToArray();
            initialized = true;
        }
    }
}
