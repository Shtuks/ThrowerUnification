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
using ThrowerUnification.Core.UnitedModdedThrowerClass;
using ThrowerUnification.Core;
using Terraria.Localization;

namespace ThrowerUnification.Content.StealthStrikes
{
    // Akira & Wardrobe Hummus - Provides a default stealth strike velocity boost for items not from the Calamity Mod.
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class DefaultStealthStrike : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.StealthStrikes;
        }

        // Define allowed mod names
        HashSet<string> allowedMods = new HashSet<string>
            {
                "BCThrower",
                "ThrowerPostGame",
                "Arsenal_Mod",
                "ThrowerArsenalAddOn",
                "SacredTools",
                "VitalityMod"
            };

        // STEALTH PROJECTILE SPEED STUFF
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.Name == "Captain's Poignard")
                return;

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

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.Name == "Captain's Poignard")
                return;

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
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.DefaultStealthStrikeTooltip"));
            }
        }

        public void AddStealthTooltip(List<TooltipLine> tooltips, string stealthTooltip)
        {
            int maxTooltipIndex = -1;
            int maxNumber = -1;

            // Find the TooltipLine with the highest TooltipX name
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Mod == "Terraria" && tooltips[i].Name.StartsWith("Tooltip"))
                {
                    // Try parse the number after "Tooltip"
                    if (int.TryParse(tooltips[i].Name.Substring(7), out int num) && num > maxNumber)
                    {
                        maxNumber = num;
                        maxTooltipIndex = i;
                    }
                }
            }

            // If found, append or set the stealthTooltip
            if (maxTooltipIndex != -1)
            {
                TooltipLine tooltip = tooltips[maxTooltipIndex];
                if (!string.IsNullOrEmpty(tooltip.Text))
                    tooltip.Text = $"{tooltip.Text}\n{stealthTooltip}";
                else
                    tooltip.Text = stealthTooltip;
            }
        }
    }
}
