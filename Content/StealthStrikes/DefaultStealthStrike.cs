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
using CalamityMod.Physics;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.DataStructures;
using CalamityMod.Projectiles;

namespace ThrowerUnification.Content.StealthStrikes
{
    // Akira & Wardrobe Hummus - Provides a default stealth strike velocity boost for items not from the Calamity Mod.
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class DefaultStealthStrike : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool IsLoadingEnabled(Mod mod)
        {
            // Only load if CalamityMod is present AND stealth strikes are enabled in your config
            return ModLoader.TryGetMod("CalamityMod", out _) && ThrowerModConfig.Instance.StealthStrikes;
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
            string[] fullSS =
            {
                "Clockwork Bomb",
                "Cactus Needle",
                "Icy Tomahawk",
                "Captain's Poignard",
                "Lodestone Javelin",
                "Valadium Throwing Axe",
                "Playing Card",
                "Chlorophyte Tomahawk",
                "Soul Bomb",
                "Soulslasher",
                "Shade Shuriken",
                "Soft Serve Sunderer",
                "White Dwarf Cutter",
                "Dracula Fang",
                "Wack Wrench"
            };

            string[] NASS =
            {
                "Blood Hunter Lexicon"
            };

            string[] crumblingSS =
            {
                "Rope Bull Whip",
                "Argonite Bull Whip",
                "Whine Vine",
                "The Worm",
                "Nerve Bundle",
                "Firelash",
                "Pricklewire",

                "Wooden Bo Staff",
                "Reinforced Bo Staff",
                "Glacial Baton",
                "Argonite Perforator",
                "Brainrot",
                "Trident of the Corruption",
                "Honey Dipper",
                "The Toothpick",
            };

            string[] crunchSS =
            {
                "Mummifier",
                "Sphinx O' Nine",
                "Tarantulash",
                "Mechanical Handful",
                "Chlorolash",
                "The Plug",
                "Jack 'O Crack",
                "The Conductor",
                "Brink of Blood",

                "Cobalt Bo Staff",
                "Palladium Bo Staff",
                "Mythril Bo Staff",
                "Orichalcum Bo Staff",
                "Adamantite Bo Staff",
                "Titanium Bo Staff",
                "Dissonance",
                "Hallowed Bo Staff",
                "Beetlegeuse",
                "Mosquito Masher",
                "Chlorophyte Bo Staff",
                "Bloodrock Lance",
                "Equilibrium",
                "Shroomite Bo Staff",
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

            if ((isNotCalamityAndConsumableRogue || isFromAllowedMod || isVanillaRogue) && !fullSS.Contains(item.Name) && !crumblingSS.Contains(item.Name) && !crunchSS.Contains(item.Name) && !NASS.Contains(item.Name))
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.DefaultStealthStrikeTooltip"));
            }

            if (crumblingSS.Contains(item.Name))
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.CrumblingStrikeTooltip"));
            }

            if (crunchSS.Contains(item.Name))
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

        // Whip tip/strike projectiles that should apply debuffs
        private static readonly string[] crumblingProj =
        {
            "RopeBullWhipProj", "ArgoniteBullWhipProj", "WhineVineProj",
            "TheWormProj", "NerveBundleProj", "FirelashProj", "PricklewireProj",
        };

        private static readonly string[] crunchProj =
        {
            "MummifierProj", "SphinxONineProj", "TarantulashProj", "MechanicalHandfulProj",
            "ChlorolashProj", "ThePlugProj", "JackOCrackProj", "TheConductorProj", "BrinkofBloodProj",
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

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            // 1) If this was spawned by another projectile, copy Calamity's stealth flag from the parent.
            Projectile parentProj = TryGetParentProjectile(source);
            if (parentProj != null)
            {
                if (IsCalamityStealthProjectile(parentProj))
                {
                    projectile.localAI[1] = 1f; // your internal debuff marker

                    // ALSO mark it as a stealth strike for Calamity
                    CalamityGlobalProjectile modProj = projectile.GetGlobalProjectile<CalamityGlobalProjectile>();
                    if (modProj != null)
                    {
                        modProj.stealthStrike = true;
                    }
                }
                return; // children do not check the player at all
            }

            // 2) This is a root whip projectile; mark it stealth if Calamity says it's stealth.
            if (IsCalamityStealthProjectile(projectile))
            {
                projectile.localAI[1] = 1f;

                CalamityGlobalProjectile modProj = projectile.GetGlobalProjectile<CalamityGlobalProjectile>();
                if (modProj != null)
                {
                    modProj.stealthStrike = true;
                }
            }
        }


        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            string projName = GetProjName(projectile.type);

            // --- whip logic stays as-is ---
            if (projectile.localAI[1] == 1f)
            {
                if (crumblingProj.Contains(projName))
                {
                    target.AddBuff(ModContent.BuffType<Crumbling>(), 300);
                }
                else if (crunchProj.Contains(projName))
                {
                    target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300);
                }
            }

            // --- bo staff logic: check directly if it's a stealth strike ---
            if (boCrumblingProj.Contains(projName) || boCrunchProj.Contains(projName))
            {
                if (IsCalamityStealthProjectile(projectile))
                {
                    if (boCrumblingProj.Contains(projName))
                    {
                        target.AddBuff(ModContent.BuffType<Crumbling>(), 300);
                        target.AddBuff(ModContent.BuffType<RiptideDebuff>(), 300);
                    }
                    else if (boCrunchProj.Contains(projName))
                    {
                        target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 300);
                        target.AddBuff(ModContent.BuffType<CrushDepth>(), 300);
                    }
                }
            }
        }



        // ---- helpers ----

        private static Projectile TryGetParentProjectile(IEntitySource source)
        {
            if (source is EntitySource_Parent parent && parent.Entity is Projectile proj)
                return proj;

            return null;
        }


        private static bool IsCalamityStealthProjectile(Projectile proj)
        {
            var calamity = ModLoader.GetMod("CalamityMod");
            if (calamity == null)
                return false;

            // Calamity API: returns true if the projectile is a stealth strike projectile
            // (object?) cast guards against null; ?? false gives a safe default
            return (bool?)calamity.Call("GetStealthProjectile", proj) ?? false;
        }

        private static string GetProjName(int type)
        {
            ModProjectile modProj = ModContent.GetModProjectile(type);
            return modProj?.Name ?? "";
        }
    }
}
