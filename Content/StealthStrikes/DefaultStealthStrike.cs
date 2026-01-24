using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.CalPlayer;
using ThrowerUnification.Core.UnitedModdedThrowerClass;
using Terraria.Localization;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.DataStructures;
using CalamityMod.Projectiles;
using Terraria;
using Terraria.ID;
using ThrowerUnification.Common.CrossmodToUMT;
using RagnarokMod.Utils;

namespace ThrowerUnification.Content.StealthStrikes
{
    // Akira & Wardrobe Hummus - Provides a default stealth strike velocity boost for items not from the Calamity Mod.
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class DefaultStealthStrike : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool IsLoadingEnabled(Mod mod) => ThrowerModConfig.Instance.StealthStrikes;

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
            if (item.ModItem != null)
            {
                if (item.ModItem.Name == "CaptainsPoniard" || item.ModItem.Name == "SteelThrowingAxe" || item.ModItem.Name == "ShinobiSlicer")
                    return;
            }

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
            string[] fullSS =
            {
                "ClockWorkBomb",
                "CactusNeedle",
                "IcyTomahawk",
                "CaptainsPoniard",
                "LodestoneJavelin",
                "ValadiumBattleAxe",
                "MagicCard",
                "ChlorophyteTomahawk",
                "SoulBomb",
                "Soulslasher",
                "BugenkaiShuriken",
                "SoftServeSunderer",
                "WhiteDwarfKunai",
                "DraculaFang",
                "WackWrench",
                "ShinobiSlicer"
            };

            string[] NASS =
            {
                "BloodHunterLexiconn"
            };

            string[] crumblingSS =
            {
                "RopeBullWhip",
                "ArgoniteBullWhip",
                "WhineVine",
                "TheWorm",
                "NerveBundle",
                "Firelash",
                "Pricklewire",

                "WoodenBoStaff",
                "ReinforcedBoStaff",
                "GlacialBaton",
                "ArgonitePerforator",
                "Brainrot",
                "TridentoftheCorruption",
                "HoneyDipper",
                "TheToothpick",
            };

            string[] crunchSS =
            {
                "Mummifier",
                "SphinxONine",
                "Tarantulash",
                "MechanicalHandful",
                "Chlorolash",
                "ThePlug",
                "JackOCrack",
                "TheConductor",
                "BrinkofBlood",

                "CobaltBoStaff",
                "PalladiumBoStaff",
                "MythrilBoStaff",
                "OrichalcumBoStaff",
                "AdamantiteBoStaff",
                "TitaniumBoStaff",
                "Dissonance",
                "HallowedBoStaff",
                "Beetlegeuse",
                "MosquitoMasher",
                "ChlorophyteBoStaff",
                "BloodrockLancee",
                "Equilibrium",
                "ShroomiteBoStaff",
                "Piercicle",
                "Purifier",
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

            if (isVanillaRogue || ThrowingToUMT.VanillaThrowerWeapons.Contains(item.type))
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.DefaultStealthStrikeTooltip"));
            }


            if (item.ModItem == null)
                return;

            if ((isNotCalamityAndConsumableRogue || isFromAllowedMod) && !fullSS.Contains(item.ModItem.Name) && !crumblingSS.Contains(item.ModItem.Name) && !crunchSS.Contains(item.ModItem.Name) && !NASS.Contains(item.ModItem.Name))
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.DefaultStealthStrikeTooltip"));
            }

            if (isMergedRogue && crumblingSS.Contains(item.ModItem.Name))
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.CrumblingStrikeTooltip"));
            }

            if (isMergedRogue && crunchSS.Contains(item.ModItem.Name))
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.CrunchStrikeTooltip"));
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

    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class DefaultStealthStrikeGlobalProjectile : GlobalProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            // Only load if CalamityMod is present AND stealth strikes are enabled in your config
            return ModLoader.TryGetMod("CalamityMod", out _) && ThrowerModConfig.Instance.StealthStrikes;
        }

        public override bool InstancePerEntity => true;

        public bool IsStealthStrikeProjectile;

        // Whip tip/strike projectiles that should apply debuffs
        private static readonly string[] crumblingProj =
        {
            "RopeBullWhipHelperProj", "RopeBullWhipProj", "ArgoniteBullWhipProj","ArgoniteBullWhipHelperProj", "WhineVineProj", "WhineVineHelperProj",
            "TheWormProj", "TheWormHelperProj", "NerveBundleProj", "NerveBundleHelperProj", "FirelashProj", "FirelashHelperProj", "PricklewireProj", "PricklewireHelperProj"
        };

        private static readonly string[] crunchProj =
        {
            "MummifierProj", "MummifierHelperProj", "SphinxONineProj", "SphinxONineHelperProj", "TarantulashProj", "TarantulashHelperProj", "MechanicalHandfulProj", "MechanicalHandfulHelperProj",
            "ChlorolashProj", "ChlorolashHelperProj", "ThePlugProj", "ThePlugHelperProj", "JackOCrackProj", "JackOCrackHelperProj", "TheConductorProj", "TheConductorHelperProj", "BrinkofBloodProj", "BrinkofBloodHelperProj"
        };

        //Bo staff versions
        private static readonly string[] boCrumblingProj =
        {
            "WoodenBoStaffProj", "ReinforcedBoStaffProj", "GlacialBatonProj", "ArgonitePerforatorProj", "BrainrotProj", "TridentoftheCorruptionProj",
            "HoneyDipperProj", "TheToothpickProj"
        };

        private static readonly string[] boCrunchProj =
        {
            "CobaltBoStaffProj", "PalladiumBoStaffProj", "MythrilBoStaffProj", "OrichalcumBoStaffProj", "AdamantiteBoStaffProj", "TitaniumBoStaffProj",
            "DissonanceProj", "HallowedBoStaffProj", "BeetlegeuseProj", "MosquitoMasherProj", "ChlorophyteBoStaffProj", "BloodrockLanceProj", "EquilibriumProj",
            "ShroomiteBoStaffProj", "PiercicleProj", "PurifierProj"
        };

        private static bool IsWhipOrBoStaffProjectile(Projectile projectile)
        {
            string projName = projectile.ModProjectile?.Name ?? projectile.Name;

            return crumblingProj.Contains(projName) ||
                   crunchProj.Contains(projName) ||
                   boCrumblingProj.Contains(projName) ||
                   boCrunchProj.Contains(projName);
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            string name = projectile.ModProjectile?.Name ?? projectile.Name;

            // Only care about our whips/bo staffs
            if (!IsWhipOrBoStaffProjectile(projectile)) return;

            Player owner = Main.player[projectile.owner];
            var calPlayer = owner.GetModPlayer<CalamityPlayer>();

            // 1️ Mark the projectile itself as stealth if spawned directly by player and available
            if (calPlayer.StealthStrikeAvailable())
            {
                IsStealthStrikeProjectile = true;
            }

            // 2️ Propagate stealth strike from parent projectile (whip helper, etc.)
            if (source is EntitySource_Parent parentSource &&
                parentSource.Entity is Projectile parentProj)
            {
                // Try to get our own GlobalProjectile from the parent
                if (parentProj.TryGetGlobalProjectile(out DefaultStealthStrikeGlobalProjectile parentStealth))
                {
                    if (parentStealth.IsStealthStrikeProjectile)
                    {
                        IsStealthStrikeProjectile = true;
                    }
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // ---- NAME GATE ----
            string name = projectile.ModProjectile?.Name ?? projectile.Name;

            bool allowed =
                crumblingProj.Contains(name) ||
                crunchProj.Contains(name) ||
                boCrumblingProj.Contains(name) ||
                boCrunchProj.Contains(name);

            if (!allowed)
                return;

            // ---- CHECK STEALTH STRIKE ----
            if (!IsStealthStrikeProjectile) return;

            // ---- APPLY DEBUFF ----
            if (crumblingProj.Contains(name) || boCrumblingProj.Contains(name))
                target.AddBuff(ModContent.BuffType<Crumbling>(), 300);
            else
                target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300);
        }
    }
}
