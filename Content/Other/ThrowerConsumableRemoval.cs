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
using ThoriumMod;
using ThoriumMod.Core.DataClasses;
using ThoriumMod.Projectiles.Thrower;
using System.Reflection;
using static ThrowerUnification.ModCompatibility;

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
            "ThoriumMod",
            "SOTSBardHealer",
            "SOTSBardHealer",
            "SpookyBardHealer",
            "SOTS"
            //"SacredTools",
            //"VitalityMod"
        };

        // CACHE

        internal static readonly HashSet<int> UnifiedThrowableTypes = new();
        internal static readonly HashSet<int> OriginallyConsumableThrowableTypes = new();

        // MATCH LOGIC

        public static bool MatchesUnifiedThrowable(Item item)
        {
            if (item == null || item.type <= ItemID.None)
                return false;

            bool isModded = item.ModItem != null;

            if (!isModded)
                return false;

            string modName = item.ModItem.Mod.Name;

            // hard restriction
            if (!AllowedMods.Contains(modName))
                return false;

            bool isMergedRogue =
                item.DamageType?.ToString() == "CalamityMod.RogueDamageClass"
                || item.DamageType == UnitedModdedThrower.Instance;

            return isMergedRogue;
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
            item.shopCustomPrice = item.value * 500;
            ForceSingleStack(item);
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

        // FORCE SINGLE STACK
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            ForceSingleStack(item);
        }

        public override void OnSpawn(Item item, IEntitySource source)
        {
            ForceSingleStack(item);
        }

        private static void ForceSingleStack(Item item)
        {
            if (item == null)
                return;

            if (UnifiedThrowableTypes.Contains(item.type))
            {
                item.stack = 1;
                item.maxStack = 1;
            }
        }

        //TOOLTIP
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item?.ModItem == null)
                return;

            string modName = item.ModItem.Mod.Name;

            if (!AllowedMods.Contains(modName))
              return;

            tooltips.RemoveAll(line =>
            {
                if (line?.Text == null)
                    return false;

                string t = line.Text.ToLowerInvariant();

                return t.Contains("not to consume") && t.Contains("thrown");
            });
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

                bool createsUnifiedThrowable = UnifiedThrowableGlobalItem.OriginallyConsumableThrowableTypes.Contains(recipe.createItem.type);

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
                        ingredient.stack *= 4;
                    }
                }
            }
        }
    }

    public class UnifiedThrowableCacheSystem : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod) => ThrowerModConfig.Instance.ConsumableWeaponConversion;

        public override void PostSetupContent()
        {
            UnifiedThrowableGlobalItem.UnifiedThrowableTypes.Clear();
            UnifiedThrowableGlobalItem.OriginallyConsumableThrowableTypes.Clear();

            foreach (Item item in ContentSamples.ItemsByType.Values)
            {
                try
                {
                    if (UnifiedThrowableGlobalItem.MatchesUnifiedThrowable(item))
                    {
                        UnifiedThrowableGlobalItem.UnifiedThrowableTypes.Add(item.type);

                        // remember items that were consumable BEFORE conversion
                        if (item.consumable)
                        {
                            UnifiedThrowableGlobalItem.OriginallyConsumableThrowableTypes.Add(item.type);
                        }
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

    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class ThoriumDropBlockerSystem : ModSystem
    {
        public override bool IsLoadingEnabled(Mod mod) => ThrowerModConfig.Instance.ConsumableWeaponConversion;

        public override void PostSetupContent()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            Type cacheHandlerType = thorium.Code.GetType("ThoriumMod.ThoriumCacheHandler");

            if (cacheHandlerType == null)
                return;

            FieldInfo cacheField = cacheHandlerType.GetField(
                "ProjItemDropCache",
                BindingFlags.Static | BindingFlags.NonPublic);

            if (cacheField == null)
                return;

            var cache = cacheField.GetValue(null) as System.Collections.IDictionary;

            if (cache == null)
                return;

            List<int> remove = new();

            foreach (System.Collections.DictionaryEntry pair in cache)
            {
                int projType = (int)pair.Key;

                // preserve pirates purse
                if (projType == ModContent.ProjectileType<PiratesPursePro1>() ||
                    projType == ModContent.ProjectileType<PiratesPursePro2>())
                {
                    continue;
                }

                object data = pair.Value;

                if (data == null)
                    continue;

                // access dropCondition through reflection
                FieldInfo dropConditionField =
                    data.GetType().GetField("dropCondition");

                if (dropConditionField != null)
                {
                    Delegate del = dropConditionField.GetValue(data) as Delegate;

                    if (del != null &&
                        del.Method.Name == "PlayerHasDartPouch")
                    {
                        continue;
                    }
                }

                remove.Add(projType);
            }

            foreach (int proj in remove)
            {
                cache.Remove(proj);
            }
        }
    }
}
