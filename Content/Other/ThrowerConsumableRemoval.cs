using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using ThrowerUnification.Core.UnitedModdedThrowerClass;
using Terraria.DataStructures;

namespace ThrowerUnification.Content.Other
{
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class UnifiedThrowableGlobalItem : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod) => ThrowerModConfig.Instance.ConsumableWeaponConversion;

        public override bool InstancePerEntity => false;

        // ALLOWED MODS

        internal static readonly HashSet<string> AllowedMods = new()
        {
            "BCThrower",
            "ThrowerPostGame",
            "Arsenal_Mod",
            "ThrowerArsenalAddOn",
            "SacredTools",
            "VitalityMod"
        };

        // CACHE

        internal static readonly HashSet<int> UnifiedThrowableTypes = new();

        // MATCH LOGIC

        public static bool MatchesUnifiedThrowable(Item item)
        {
            if (item == null || item.type <= ItemID.None)
                return false;

            bool isModded = item.ModItem != null;

            string modName = isModded
                ? item.ModItem.Mod.Name
                : null;

            bool isMergedRogue =
                item.DamageType?.ToString() == "CalamityMod.RogueDamageClass"
                || item.DamageType == UnitedModdedThrower.Instance;

            bool isNotCalamityAndConsumableRogue =
                isModded
                && modName != "CalamityMod"
                && modName != "CatalystMod"
                && modName != "CalamityHunt"
                && item.consumable
                && isMergedRogue;

            bool isFromAllowedMod =
                isModded
                && AllowedMods.Contains(modName)
                && isMergedRogue;

            return isNotCalamityAndConsumableRogue
                || isFromAllowedMod;
        }

        // ITEM CONVERSION

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return UnifiedThrowableTypes.Contains(entity.type);
        }

        public override void SetDefaults(Item item)
        {
            item.consumable = false;
            item.maxStack = 1;
            item.notAmmo = true;
        }

        public override bool CanReforge(Item item)
        {
            if (UnifiedThrowableTypes.Contains(item.type))
                return true;

            return base.CanReforge(item);
        }

        // PREVENT CONSUMPTION

        public override bool ConsumeItem(Item item, Player player)
        {
            if (UnifiedThrowableTypes.Contains(item.type))
                return false;

            return true;
        }

        // PREVENT AMMO CONSUMPTION

        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
        {
            if (UnifiedThrowableTypes.Contains(ammo.type))
                return false;

            return base.CanConsumeAmmo(weapon, ammo, player);
        }
    }

    // RECIPE EDITS

    public class UnifiedThrowableRecipeSystem : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod) => ThrowerModConfig.Instance.ConsumableWeaponConversion;

        public override void PostAddRecipes()
        {
            for (int i = 0; i < Main.recipe.Length; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe == null)
                    continue;

                bool createsUnifiedThrowable =
                    UnifiedThrowableGlobalItem.UnifiedThrowableTypes.Contains(recipe.createItem.type);

                if (!createsUnifiedThrowable)
                    continue;

                // Force output to 1
                recipe.createItem.stack = 1;

                foreach (Item ingredient in recipe.requiredItem)
                {
                    if (ingredient == null || ingredient.type <= ItemID.None)
                        continue;

                    if (UnifiedThrowableGlobalItem.UnifiedThrowableTypes.Contains(ingredient.type))
                    {
                        ingredient.stack = 1;
                    }
                    else
                    {
                        ingredient.stack *= 5;
                    }
                }
            }
        }
    }

    // NPC DROP EDITS

    public class UnifiedThrowableGlobalNPC : GlobalNPC
    {
        public override bool IsLoadingEnabled(Mod mod) => ThrowerModConfig.Instance.ConsumableWeaponConversion;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            foreach (IItemDropRule rule in npcLoot.Get())
            {
                TryModifyRule(rule);
            }
        }

        private void TryModifyRule(IItemDropRule rule)
        {
            if (rule == null)
                return;

            // COMMON DROP

            if (rule is CommonDrop commonDrop)
            {
                Item item = ContentSamples.ItemsByType[commonDrop.itemId];

                if (UnifiedThrowableGlobalItem.UnifiedThrowableTypes.Contains(item.type))
                {
                    commonDrop.amountDroppedMinimum = 1;
                    commonDrop.amountDroppedMaximum = 1;
                }
            }

            // RECURSIVE CHAINS

            if (rule.ChainedRules == null)
                return;

            foreach (var chained in rule.ChainedRules)
            {
                TryModifyRule(chained.RuleToChain);
            }
        }
    }

    public class UnifiedThrowableCacheSystem : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod) => ThrowerModConfig.Instance.ConsumableWeaponConversion;

        public override void PostSetupContent()
        {
            UnifiedThrowableGlobalItem.UnifiedThrowableTypes.Clear();

            foreach (Item item in ContentSamples.ItemsByType.Values)
            {
                try
                {
                    if (UnifiedThrowableGlobalItem.MatchesUnifiedThrowable(item))
                    {
                        UnifiedThrowableGlobalItem.UnifiedThrowableTypes.Add(item.type);
                    }
                }
                catch
                {
                }
            }
        }
    }

    public class UnifiedThrowableGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool shouldBlockItemDrop;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse itemSource &&
                itemSource.Item != null &&
                UnifiedThrowableGlobalItem.UnifiedThrowableTypes.Contains(itemSource.Item.type))
            {
                shouldBlockItemDrop = true;
            }
        }

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (shouldBlockItemDrop)
            {
                projectile.noDropItem = true;
            }
        }
    }
}
