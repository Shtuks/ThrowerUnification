using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core;
using Terraria.Localization;
using ThoriumMod.Items.BossLich;
using ThoriumMod.Items.Icy;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.BossThePrimordials.Aqua;
using ThoriumMod.Items.Lodestone;
using ThoriumMod.Items.Valadium;
using ThoriumMod.Items.BossViscount;
using ThoriumMod.Items.ArcaneArmor;
using ThoriumMod.Items.Depths;
using ThoriumMod.Items.Steel;
using ThrowerUnification.Content.Projectiles.StealthPro;

namespace ThrowerUnification.Content.StealthStrikes.ThoriumStealthStrikes
{
    //Akira & Wardrobe Hummus
    public enum StealthStrikeType
    {
        None,
        CactusNeedle,
        IcyTomahawk,
        ZephyrsRuin,
        ClockworkBomb,
        SoulBomb,
        PlayingCard,
        WhiteDwarfCutter,
        Soulslasher,
        CaptainsPoignard,
        SoftServeSunderer,
        TerraKnife,
        TerraKnife2,
        ShadeShuriken,
        GelGlove,
        TidalWave,
        LodestoneJavelin,
        ValadiumAxe,
        ChlorophyteTomahawk,
        FireAxe,
        DraculaFang,
        WackWrench,
        EnchantedKnife,
        GoblinWarSpear,
        MeteoriteClusterBomb,
        AquaiteKnife,
        SteelThrowingAxe,
        DurasteelThrowingSpear,
    }
    [ExtendsFromMod(ModCompatibility.Thorium.Name, ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name, ModCompatibility.Calamity.Name)]
    public class ThoriumStealthStrikeSetup : GlobalItem
    {
        //Don't load the stealth strikes if Infernal or Hummus' mod is enabled to prevent overlap or duplication.
        public override bool IsLoadingEnabled(Mod mod)
        {
            // Only load if CalamityMod is present AND stealth strikes are enabled in your config
            return ModLoader.TryGetMod("CalamityMod", out _) && ThrowerModConfig.Instance.StealthStrikes;
        }

        public override bool InstancePerEntity => true;

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var calPlayer = player.GetModPlayer<CalamityPlayer>();

            // ===================== ICY TOMAHAWK =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<IcyTomahawk>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    Vector2[] velocities = {
                        velocity * 0.33f,
                        velocity * 0.66f,
                        velocity,
                        velocity * 1.33f,
                    };

                    foreach (Vector2 v in velocities)
                    {
                        int projID = Projectile.NewProjectile(
                            source,
                            position,
                            v,
                            type,
                            damage,
                            knockback,
                            player.whoAmI
                        );

                        if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                        {
                            stealthGlobal.SetupAsStealthStrike(StealthStrikeType.IcyTomahawk);
                        }
                    }

                    return false;
                }
            }

            // ===================== CACTUS NEEDLE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<CactusNeedle>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    float spread = MathHelper.ToRadians(5f);

                    for (int i = -1; i <= 1; i++) // 3 projectiles: left, center, right
                    {
                        float rotation = spread * i;
                        Vector2 newVelocity = velocity.RotatedBy(rotation);

                        int projID = Projectile.NewProjectile(
                            source,
                            position,
                            newVelocity,
                            type,
                            damage,
                            knockback,
                            player.whoAmI
                        );

                        if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                        {
                            stealthGlobal.SetupAsStealthStrike(StealthStrikeType.CactusNeedle);
                        }
                    }

                    return false;
                }
            }

            // ===================== GEL GLOVE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<GelGlove>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.GelGlove);
                    }

                    return false; // Prevent default projectile since we spawned our own
                }
            }

            // ===================== ZEPHYR'S RUIN =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumRework" && item.ModItem.Name == "ZephyrsRuin")
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    velocity *= 1.75f;
                    damage = (int)(damage * 1.5f);

                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.ZephyrsRuin);
                    }

                    return false;
                }
            }

            // ===================== CAPTAIN'S POIGNARD =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<CaptainsPoniard>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) &&
                        Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.CaptainsPoignard);
                    }

                    return false; // prevent original spawn, we spawned manually
                }
            }

            // ===================== LODESTONE JAVELIN =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<LodestoneJavelin>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.LodestoneJavelin);
                    }

                    return false; // Prevent default behavior
                }
            }


            // ===================== VALADIUM BATTLE AXE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<ValadiumBattleAxe>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.ValadiumAxe);
                    }

                    return false; // Prevent default behavior
                }
            }

            // ===================== CLOCKWORK BOMB =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<ClockWorkBomb>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.ClockworkBomb);
                    }

                    return false;
                }
            }

            // ===================== CHLOROPHYTE TOMAHAWK =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<ChlorophyteTomahawk>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    velocity *= 0.75f;
                    damage = (int)(damage * 1f);

                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.ChlorophyteTomahawk);
                    }

                    return false;
                }
            }

            // ===================== SOUL BOMB =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<SoulBomb>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.SoulBomb);
                    }

                    return false;
                }
            }

            // ===================== PLAYING CARD =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<MagicCard>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int count = 5;
                    float spread = MathHelper.ToRadians(10f);
                    float baseAngle = velocity.ToRotation();
                    float startAngle = baseAngle - spread / 2f;

                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) &&
                        thoriumMod.TryFind("MagicCardPro", out ModProjectile modProj))
                    {
                        int projType = modProj.Type;

                        for (int i = 0; i < count; i++)
                        {
                            float rotation = MathHelper.ToRadians(15f) * (i - 1.5f);
                            Vector2 rotatedVelocity = velocity.RotatedBy(rotation);

                            int adjustedDamage = (int)Math.Round(damage * 1.15);

                            int projID = Projectile.NewProjectile(
                                source,
                                position,
                                rotatedVelocity,
                                projType,
                                adjustedDamage,
                                knockback,
                                player.whoAmI,
                                4,
                                1f // force explosive version
                            );

                            if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                            {
                                stealthGlobal.SetupAsStealthStrike(StealthStrikeType.PlayingCard);
                            }
                        }

                        return false;
                    }
                }
            }

            // ===================== SHADE SHURIKEN =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<BugenkaiShuriken>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                        thorium.TryFind("ShadeShuriken", out ModProjectile modProj))
                    {
                        int projType = modProj.Type;

                        int projID = Projectile.NewProjectile(
                            source,
                            position,
                            velocity * 1.0f, // Faster
                            projType,
                            (int)(damage * 1.0f), // Stronger
                            knockback,
                            player.whoAmI
                        );

                        if (Main.projectile.IndexInRange(projID) &&
                            Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                        {
                            stealthGlobal.SetupAsStealthStrike(StealthStrikeType.ShadeShuriken);
                        }
                    }

                    return false;
                }
            }

            // ===================== SOULSLASHER =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<Soulslasher>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) &&
                        thoriumMod.TryFind("SoulslasherPro", out ModProjectile modProj))
                    {
                        int projType = modProj.Type;

                        int projID = Projectile.NewProjectile(
                            source,
                            position,
                            velocity,
                            projType,
                            damage,
                            knockback,
                            player.whoAmI
                        );

                        if (Main.projectile.IndexInRange(projID) &&
                            Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                        {
                            stealthGlobal.SetupAsStealthStrike(StealthStrikeType.Soulslasher);
                        }

                        return false; // Prevent default projectile
                    }
                }
            }

            // ===================== SOFT SERVE SUNDERER =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<SoftServeSunderer>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) &&
                        Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.SoftServeSunderer);
                    }

                    return false;
                }
            }

            // ===================== WHITE DWARF CUTTER =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<WhiteDwarfKunai>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    // Fire the main kunai
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) &&
                        Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.WhiteDwarfCutter);
                    }

                    // Spawn the two angled kunai
                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) &&
                        thorium.TryFind("WhiteDwarfKunaiPro2", out ModProjectile sideProjMod))
                    {
                        int sideProjType = sideProjMod.Type;

                        Vector2 velocityUp = velocity.RotatedBy(MathHelper.ToRadians(5f));
                        Vector2 velocityDown = velocity.RotatedBy(MathHelper.ToRadians(-5f));

                        int upProjID = Projectile.NewProjectile(source, position, velocityUp, sideProjType, damage, knockback, player.whoAmI);
                        int downProjID = Projectile.NewProjectile(source, position, velocityDown, sideProjType, damage, knockback, player.whoAmI);

                        if (Main.projectile.IndexInRange(upProjID) &&
                            Main.projectile[upProjID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles upStealthGlobal))
                        {
                            upStealthGlobal.SetupAsStealthStrike(StealthStrikeType.WhiteDwarfCutter);
                        }

                        if (Main.projectile.IndexInRange(downProjID) &&
                            Main.projectile[downProjID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles downStealthGlobal))
                        {
                            downStealthGlobal.SetupAsStealthStrike(StealthStrikeType.WhiteDwarfCutter);
                        }
                    }

                    return false;
                }
            }

            // ===================== TERRA KNIFE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<TerraKnife>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
                    thorium.TryFind("TerraKnifePro", out ModProjectile mainPro);
                    thorium.TryFind("TerraKnifePro2", out ModProjectile sidePro);

                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        sidePro.Type,
                        (int)(damage * 0.8),
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) &&
                        Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.TerraKnife);
                    }

                    int sideProjType = mainPro.Type;

                    Vector2 velocityUp = velocity.RotatedBy(MathHelper.ToRadians(5f));
                    Vector2 velocityDown = velocity.RotatedBy(MathHelper.ToRadians(-5f));

                    int upProjID = Projectile.NewProjectile(source, position, velocityUp, sideProjType, damage, knockback, player.whoAmI);
                    int downProjID = Projectile.NewProjectile(source, position, velocityDown, sideProjType, damage, knockback, player.whoAmI);

                    if (Main.projectile.IndexInRange(upProjID) &&
                        Main.projectile[upProjID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles upStealthGlobal))
                    {
                        upStealthGlobal.SetupAsStealthStrike(StealthStrikeType.TerraKnife2);
                    }

                    if (Main.projectile.IndexInRange(downProjID) &&
                        Main.projectile[downProjID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles downStealthGlobal))
                    {
                        downStealthGlobal.SetupAsStealthStrike(StealthStrikeType.TerraKnife2);
                    }

                    return false;
                }
            }

            // ===================== TIDAL WAVE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<TidalWave>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<TidalWaveWhirlpool>(), damage / 2, knockback, player.whoAmI);

                    if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.TidalWave);
                    }

                    return false;
                }
            }

            // ===================== FIRE AXE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<FireAxe>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity * 1.5f,
                        type,
                        (int)(damage * 1f),
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) &&
                        Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.FireAxe);
                    }

                    return false;
                }
            }

            // ===================== DRACULA FANG =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<DraculaFang>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int numProjectiles = 4;              // how many fangs to shoot
                    float totalSpread = MathHelper.ToRadians(35f); // total cone angle
                    for (int i = 0; i < numProjectiles; i++)
                    {
                        // Random angle within [-totalSpread/2, totalSpread/2]
                        float offset = Main.rand.NextFloat(-totalSpread / 2f, totalSpread / 2f);

                        // Rotate the velocity by this offset
                        Vector2 perturbedSpeed = velocity.RotatedBy(offset);

                        int projID = Projectile.NewProjectile(
                            source,
                            position,
                            perturbedSpeed,
                            type,
                            damage,
                            knockback,
                            player.whoAmI
                        );

                        if (Main.projectile.IndexInRange(projID) &&
                            Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                        {
                            stealthGlobal.SetupAsStealthStrike(StealthStrikeType.DraculaFang);

                            // Give stealth strike fangs local iframes
                            Projectile proj = Main.projectile[projID];
                            proj.usesLocalNPCImmunity = true;
                            proj.localNPCHitCooldown = 30;
                        }
                    }

                    return false;
                }
            }

            // ===================== WACK WRENCH =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<WackWrench>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) &&
                        Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.WackWrench);

                        // Apply stealth strike properties right away
                        Projectile proj = Main.projectile[projID];

                        proj.scale = 6f;
                        proj.width *= 6;
                        proj.height *= 6;
                        proj.position -= new Vector2(proj.width / 2f, proj.height / 2f);

                        proj.tileCollide = false; // goes through walls
                    }

                    return false;
                }
            }

            // ===================== ENCHANTED KNIFE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<EnchantedKnife>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.EnchantedKnife);
                    }

                    return false; // prevent default behavior; we spawned our stealth projectile
                }
            }

            // ===================== GOBLIN WAR SPEAR =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<GoblinWarSpear>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.GoblinWarSpear);
                    }

                    return false; // prevent normal shot
                }
            }

            // ===================== METEORITE CLUSTER BOMB =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<MeteoriteClusterBomb>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) &&
                        Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.MeteoriteClusterBomb);
                    }

                    return false; // prevent default projectile
                }
            }

            // ===================== AQUAITE KNIFE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<AquaiteKnife>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int projID = Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        type,
                        damage,
                        knockback,
                        player.whoAmI
                    );

                    if (Main.projectile.IndexInRange(projID) &&
                        Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                    {
                        stealthGlobal.SetupAsStealthStrike(StealthStrikeType.AquaiteKnife);
                    }

                    return false; // prevent default projectile
                }
            }

            // ===================== STEEL THROWING AXE =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<SteelThrowingAxe>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int count = 3;
                    float angleStep = MathHelper.ToRadians(15f); // 15° each side
                    float middleOffset = (count - 1) / 2f;

                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) &&
                        thoriumMod.TryFind("SteelThrowingAxePro", out ModProjectile modProj))
                    {
                        int projType = modProj.Type;

                        for (int i = 0; i < count; i++)
                        {
                            float rotation = (i - middleOffset) * angleStep;
                            Vector2 rotatedVelocity = velocity.RotatedBy(rotation);

                            int adjustedDamage = (int)Math.Round(damage * 1f);

                            int projID = Projectile.NewProjectile(
                                source,
                                position,
                                rotatedVelocity,
                                projType,
                                adjustedDamage,
                                knockback,
                                player.whoAmI
                            );

                            if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                            {
                                stealthGlobal.SetupAsStealthStrike(StealthStrikeType.SteelThrowingAxe);
                            }
                        }

                        return false;
                    }
                }
            }

            // ===================== DURASTEEL THROWING SPEAR =====================
            if (item.ModItem != null && item.ModItem.Mod.Name == "ThoriumMod" && item.type == ModContent.ItemType<DurasteelThrowingSpear>())
            {
                if (calPlayer.StealthStrikeAvailable())
                {
                    int count = 3;
                    float angleStep = MathHelper.ToRadians(15f); // 15° each side
                    float middleOffset = (count - 1) / 2f;

                    if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod) &&
                        thoriumMod.TryFind("DurasteelThrowingSpearPro", out ModProjectile modProj))
                    {
                        int projType = modProj.Type;

                        for (int i = 0; i < count; i++)
                        {
                            float rotation = (i - middleOffset) * angleStep;
                            Vector2 rotatedVelocity = velocity.RotatedBy(rotation);

                            int adjustedDamage = (int)Math.Round(damage * 1f);

                            int projID = Projectile.NewProjectile(
                                source,
                                position,
                                rotatedVelocity,
                                projType,
                                adjustedDamage,
                                knockback,
                                player.whoAmI
                            );

                            if (Main.projectile.IndexInRange(projID) && Main.projectile[projID].TryGetGlobalProjectile(out ThoriumStealthStrikeProjectiles stealthGlobal))
                            {
                                stealthGlobal.SetupAsStealthStrike(StealthStrikeType.DurasteelThrowingSpear);
                            }
                        }

                        return false;
                    }
                }
            }

            return true;
        }

        //EXHAUSTION REMOVAL
        public override void SetDefaults(Item item)
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) return;

            //TERRA KNIFE
            if (item.type == thorium.Find<ModItem>("TerraKnife").Type)
            {
                TrySetIsThrowerNon(item, false);
            }

            //GEL GLOVE 
            if (item.type == thorium.Find<ModItem>("GelGlove").Type)
            {
                TrySetIsThrowerNon(item, false);
            }

            //TIDAL WAVE
            if (item.type == thorium.Find<ModItem>("TidalWave").Type)
            {
                TrySetIsThrowerNon(item, false);
            }

            //FIRE AXE
            if (item.type == thorium.Find<ModItem>("FireAxe").Type)
            {
                TrySetIsThrowerNon(item, false);
            }
        }

        private void TrySetIsThrowerNon(Item item, bool active)
        {
            try
            {
                if (item.ModItem == null)
                {
                    Main.NewText("No ModItem attached");
                    return;
                }

                Type modItemType = item.ModItem.GetType();

                // Try field first
                FieldInfo field = modItemType.GetField("isThrowerNon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    field.SetValue(item.ModItem, active);
                    //Main.NewText($"[Field] Set healAmount of {item.Name} to {newCost}");
                    return;
                }

                // Then try property
                PropertyInfo prop = modItemType.GetProperty("isThrowerNon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(item.ModItem, active);
                    return;
                }

                //Main.NewText("healAmount not found on ModItem.");
            }
            catch (Exception)
            {
                //Main.NewText($"Error setting healAmount: {ex.Message}");
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

        public void FullTooltipOveride(List<TooltipLine> tooltips, string stealthTooltip)
        {
            for (int index = 0; index < tooltips.Count; ++index)
            {
                if (tooltips[index].Mod == "Terraria")
                {
                    if (tooltips[index].Name == "Tooltip0")
                    {
                        TooltipLine tooltip = tooltips[index];
                        tooltip.Text = $"{stealthTooltip}";
                    }
                    else if (tooltips[index].Name.Contains("Tooltip"))
                    {
                        tooltips[index].Hide();
                    }
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModContent.ItemType<CactusNeedle>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.CactusNeedle"));
            }

            if (item.type == ModContent.ItemType<IcyTomahawk>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.IcyTomahawk"));
            }

            if (ModLoader.TryGetMod("ThoriumRework", out Mod thorRework))
            {
                if (item.type == thorRework.Find<ModItem>("ZephyrsRuin").Type)
                    AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.ZephyrRuin"));
            }

            if (item.type == ModContent.ItemType<ClockWorkBomb>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.ClockworkBomb"));
            }

            if (item.type == ModContent.ItemType<SoulBomb>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.SoulBomb"));
            }

            if (item.type == ModContent.ItemType<MagicCard>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.PlayingCard"));
            }

            if (item.type == ModContent.ItemType<Soulslasher>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.Soulslasher"));
            }

            if (item.type == ModContent.ItemType<WhiteDwarfKunai>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.WhiteDwarfCutter"));
            }

            if (item.type == ModContent.ItemType<CaptainsPoniard>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.CaptainsPoignard"));
            }

            if (item.type == ModContent.ItemType<SoftServeSunderer>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.SoftServeSunderer"));
            }

            if (item.type == ModContent.ItemType<BugenkaiShuriken>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.ShadeShuriken"));
            }

            if (item.type == ModContent.ItemType<LodestoneJavelin>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.LodestoneJav"));
            }

            if (item.type == ModContent.ItemType<ValadiumBattleAxe>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.ValadiumThrowingAxe"));
            }

            if (item.type == ModContent.ItemType<ChlorophyteTomahawk>())
            {
                AddStealthTooltip(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.ChlorophyteTomahawk"));
            }

            if (item.type == ModContent.ItemType<TerraKnife>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.TerraKnife"));
            }

            if (item.type == ModContent.ItemType<TidalWave>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.TidalWave"));
            }

            if (item.type == ModContent.ItemType<GelGlove>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.GelGlove"));
            }

            if (item.type == ModContent.ItemType<FireAxe>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.FireAxe"));
            }

            if (item.type == ModContent.ItemType<DraculaFang>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.DraculaFang"));
            }

            if (item.type == ModContent.ItemType<WackWrench>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.WackWrench"));
            }

            if (item.type == ModContent.ItemType<EnchantedKnife>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.EnchantedKnife"));
            }

            if (item.type == ModContent.ItemType<GoblinWarSpear>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.GoblinWarSpear"));
            }

            if (item.type == ModContent.ItemType<MeteoriteClusterBomb>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.MeteoriteClusterBomb"));
            }

            if (item.type == ModContent.ItemType<AquaiteKnife>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.AquaiteKnife"));
            }

            if (item.type == ModContent.ItemType<SteelThrowingAxe>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.SteelThrowingAxe"));
            }

            if (item.type == ModContent.ItemType<DurasteelThrowingSpear>())
            {
                FullTooltipOveride(tooltips, Language.GetTextValue("Mods.ThrowerUnification.ThoriumStealthStrike.DurasteelThrowingSpear"));
            }
        }
    }
}
