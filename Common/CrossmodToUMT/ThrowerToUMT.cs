using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Common.CrossmodToUMT
{
    public class ThrowingToUMT : GlobalItem
    {
        public static readonly int[] VanillaThrowerWeapons =
        [
            ItemID.Shuriken,
            ItemID.ThrowingKnife,
            ItemID.PoisonedKnife,
            ItemID.Snowball,
            ItemID.BoneDagger,
            ItemID.BoneJavelin,
            ItemID.StarAnise,
            ItemID.SpikyBall,
            ItemID.RottenEgg,
            ItemID.Javelin,
            ItemID.FrostDaggerfish,
            ItemID.AleThrowingGlove,
            ItemID.MolotovCocktail,
        ];

        public override void SetDefaults(Item item)
        {
            if ((item.DamageType == DamageClass.Throwing && ThrowerModConfig.Instance.Vanilla) || (VanillaThrowerWeapons.Contains(item.type) && ThrowerModConfig.Instance.LegacyVanillaThrowerWeapons))
            {
                item.DamageType = ModContent.GetInstance<UnitedModdedThrower>();
            } 
        }
    }
    public class ThrowingToUMTProj : GlobalProjectile
    {
        public static readonly int[] VanillaThrowerWeaponsProjs =
        [
            ProjectileID.Shuriken,
            ProjectileID.ThrowingKnife,
            ProjectileID.PoisonedKnife,
            ProjectileID.SnowBallFriendly,
            ProjectileID.BoneDagger,
            ProjectileID.BoneJavelin,
            ProjectileID.StarAnise,
            ProjectileID.SpikyBall,
            ProjectileID.Bone,
            ProjectileID.RottenEgg,
            ProjectileID.JavelinFriendly,
            ProjectileID.FrostDaggerfish,
            ProjectileID.Ale,
            ProjectileID.MolotovCocktail,
            ProjectileID.MolotovFire,
            ProjectileID.MolotovFire2,
            ProjectileID.MolotovFire3,
        ];

        public override void SetDefaults(Projectile item)
        {
            if ((item.DamageType == DamageClass.Throwing && ThrowerModConfig.Instance.Vanilla) || (VanillaThrowerWeaponsProjs.Contains(item.type) && ThrowerModConfig.Instance.LegacyVanillaThrowerWeapons))
            {
                item.DamageType = ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }
}