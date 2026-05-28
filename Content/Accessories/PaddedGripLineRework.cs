using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThrowerUnification.Core.UnitedModdedThrowerClass;
using ThoriumMod.Buffs.Thrower;

namespace ThrowerUnification.Content.Accessories
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class PaddedGripLineRework : ModPlayer
    {
        public int rogueSpeedCooldown;

        public override bool IsLoadingEnabled(Mod mod) =>
            ThrowerModConfig.Instance.Calamity &&
            ThrowerModConfig.Instance.ConsumableWeaponConversion;

        public static bool PlayerHasPaddedGrip(Player player) =>
            player.GetModPlayer<ThoriumPlayer>().paddedGrip;

        public static bool PlayerHasBoneGrip(Player player) =>
            player.GetModPlayer<ThoriumPlayer>().boneGrip;

        public static bool PlayerHasMagnetoGrip(Player player) =>
            player.GetModPlayer<ThoriumPlayer>().magnetoGrip;

        public override void ResetEffects()
        {
            if (rogueSpeedCooldown > 0)
                rogueSpeedCooldown--;
        }
    }

    public class PaddedGripProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => false;

        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player player = Main.player[projectile.owner];

            if (player == null || !player.active)
                return;

            // Check accessory ownership
            bool hasGrip =
                PaddedGripLineRework.PlayerHasPaddedGrip(player) ||
                PaddedGripLineRework.PlayerHasBoneGrip(player) ||
                PaddedGripLineRework.PlayerHasMagnetoGrip(player);

            if (!hasGrip)
                return;

            // Detect unified thrower / rogue projectiles
            bool validProjectile =
                projectile.DamageType == UnitedModdedThrower.Instance ||
                projectile.DamageType?.ToString() == "CalamityMod.RogueDamageClass" ||
                projectile.DamageType == DamageClass.Throwing;

            if (!validProjectile)
                return;

            var modPlayer = player.GetModPlayer<PaddedGripLineRework>();

            // global cooldown
            if (modPlayer.rogueSpeedCooldown > 0)
                return;

            int procChance = 10;
            int cooldown = 120;
            int buffTime = 60;

            // strongest first
            if (PaddedGripLineRework.PlayerHasMagnetoGrip(player))
            {
                procChance = 5;
                cooldown = 90;
                buffTime = 60;
            }
            else if (PaddedGripLineRework.PlayerHasBoneGrip(player))
            {
                procChance = 5;
                cooldown = 120; 
                buffTime = 60;
            }
            else if (PaddedGripLineRework.PlayerHasPaddedGrip(player))
            {
                procChance = 10;
                cooldown = 120;
                buffTime = 60;
            }

            if (Main.rand.NextBool(procChance))
            {
                player.AddBuff(ModContent.BuffType<ThrowingSpeed>(), buffTime);

                modPlayer.rogueSpeedCooldown = cooldown;
            }
        }
    }
}