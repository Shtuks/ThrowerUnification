using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Common.CrossmodToUMT
{
    public class ThrowingToUMT : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.LegacyVanillaThrowerWeapons;
        }
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Shuriken || item.type == ItemID.ThrowingKnife || item.type == ItemID.PoisonedKnife || item.type == ItemID.Snowball
                || item.type == ItemID.Beenade || item.type == ItemID.BoneDagger || item.type == ItemID.BoneJavelin
                || item.type == ItemID.Grenade || item.type == ItemID.StickyGrenade || item.type == ItemID.BouncyGrenade
                || item.type == ItemID.StarAnise || item.type == ItemID.SpikyBall || item.type == ItemID.Bone
                || item.type == ItemID.RottenEgg || item.type == ItemID.Javelin || item.type == ItemID.FrostDaggerfish
                || item.type == ItemID.PartyGirlGrenade || item.type == ItemID.AleThrowingGlove || item.type == ItemID.MolotovCocktail)
            { 
                item.DamageType = ModContent.GetInstance<UnitedModdedThrower>();
            }

            if (item.DamageType == DamageClass.Throwing)
            {
                item.DamageType = (DamageClass)(object)ModContent.GetInstance<UnitedModdedThrower>();
            } 
        }
    }
    public class ThrowingToUMTProj : GlobalProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.LegacyVanillaThrowerWeapons;
        }
        public override void SetDefaults(Projectile item)
        {
            if (item.type == ProjectileID.Shuriken || item.type == ProjectileID.ThrowingKnife || item.type == ProjectileID.PoisonedKnife
                 || item.type == ProjectileID.SnowBallFriendly || item.type == ProjectileID.Beenade || item.type == ProjectileID.BoneDagger
                  || item.type == ProjectileID.BoneJavelin || item.type == ProjectileID.Grenade || item.type == ProjectileID.StickyGrenade
                   || item.type == ProjectileID.BouncyGrenade || item.type == ProjectileID.StarAnise || item.type == ProjectileID.SpikyBall
                    || item.type == ProjectileID.Bone || item.type == ProjectileID.RottenEgg || item.type == ProjectileID.JavelinFriendly
                     || item.type == ProjectileID.FrostDaggerfish || item.type == ProjectileID.PartyGirlGrenade || item.type == ProjectileID.Ale
                      || item.type == ProjectileID.MolotovCocktail || item.type == ProjectileID.MolotovFire || item.type == ProjectileID.MolotovFire2
                       || item.type == ProjectileID.MolotovFire3)
            {
                item.DamageType = ModContent.GetInstance<UnitedModdedThrower>();
            }

            if (item.DamageType == DamageClass.Throwing)
            {
                item.DamageType = (DamageClass)(object)ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }
}