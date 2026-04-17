using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ID;
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
        public override void PostUpdateEquips()
        {
            if (ThrowerModConfig.Instance.Calamity)
            {
                if (Player.armor[1].type == ItemID.MonkShirt)
                {
                    Player.GetDamage<RogueDamageClass>() -= 0.2f;
                    Player.GetDamage(DamageClass.Throwing) += 0.2f;
                }
                if (Player.armor[2].type == ItemID.MonkPants)
                {
                    Player.GetCritChance<RogueDamageClass>() -= 15;
                    Player.GetCritChance(DamageClass.Throwing) += 15;
                }

                if (Player.armor[0].type == ItemID.MonkAltHead)
                {
                    Player.GetDamage<RogueDamageClass>() -= 0.2f;
                    Player.GetDamage(DamageClass.Throwing) += 0.2f;
                }
                if (Player.armor[1].type == ItemID.MonkAltShirt)
                {
                    Player.GetCritChance<RogueDamageClass>() -= 5;
                    Player.GetCritChance(DamageClass.Throwing) += 5;
                }
                if (Player.armor[2].type == ItemID.MonkAltPants)
                {
                    Player.GetCritChance<RogueDamageClass>() -= 20;
                    Player.GetCritChance(DamageClass.Throwing) += 20;
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            Player player = Main.LocalPlayer;
            var CalPlayer = player.GetModPlayer<CalamityPlayer>();

            ApplyRogueUseTimeFix();

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
                        "Wack Wrench",
                        "Goblin War Spear",
                        "Meteorite Cluster Bomb",
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

        private void ApplyRogueUseTimeFix()
        {
            var calamityPlayer = base.Player.Calamity();
            Item it = !Main.mouseItem.IsAir ? Main.mouseItem : Player.HeldItem;
            if (!calamityPlayer.wearingRogueArmor || it.useAnimation == it.useTime) // When the player does not wear a rogue armor or the item animation and usetime is the same, the fix does not have to be applied.
            {
                return;
            }
            bool flag = it.damage > 0;
            bool hasHitboxes = !it.noMelee || it.shoot > ProjectileID.None;
            bool flag2 = it.pick > 0;
            bool isAxe = it.axe > 0;
            bool isHammer = it.hammer > 0;
            bool isPlaced = it.createTile != -1;
            bool isChannelable = it.channel;
            bool hasNonWeaponFunction = flag2 || isAxe || isHammer || isPlaced || isChannelable;
            bool playerUsingWeapon = flag && hasHitboxes && !hasNonWeaponFunction;
            if ((it.IsAir || !it.CountsAsClass<RogueDamageClass>()) && calamityPlayer.GemTechSet && calamityPlayer.GemTechState.IsRedGemActive)
            {
                return;
            }

            // Besides Rogue attackspeed, Thoriums own attack speed changes for throwing and general class have to be calculated into the attackspeed.
            float attackSpeed = base.Player.GetAttackSpeed<RogueDamageClass>() * base.Player.GetAttackSpeed(DamageClass.Throwing) * base.Player.GetAttackSpeed(DamageClass.Generic);
            int adjustedUseTime = (int)(it.useTime / attackSpeed); // Calculates the effective use time

            bool animationCheck = (base.Player.itemTime == adjustedUseTime);
            if (!calamityPlayer.stealthStrikeThisFrame && animationCheck)
            {
                if (calamityPlayer.StealthStrikeAvailable())
                {
                    calamityPlayer.ConsumeStealthByAttacking();
                    return;
                }
                calamityPlayer.rogueStealth = 0f;
            }
        }
    }
}
