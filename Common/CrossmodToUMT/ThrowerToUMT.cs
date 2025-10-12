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
            return ThrowerModConfig.Instance.Vanilla;
        }
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Shuriken 
           //|| item.type == ItemID.Snowball
               )
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
            return ThrowerModConfig.Instance.Vanilla;
        }
        public override void SetDefaults(Projectile item)
        {
            if (item.type == ProjectileID.Shuriken)
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