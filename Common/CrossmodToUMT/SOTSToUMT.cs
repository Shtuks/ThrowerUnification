using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Common.CrossmodToUMT
{
    [ExtendsFromMod(ModCompatibility.SOTS.Name)]
    [JITWhenModsEnabled(ModCompatibility.SOTS.Name)]
    public class SOTSToUMT : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.Vanilla;
        }

        public override void SetDefaults(Item entity)
        {
            if (entity.type == ModContent.ItemType<SOTS.Items.Slime.GelAxe>())
            {
                entity.DamageType = ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }

    [ExtendsFromMod(ModCompatibility.SOTS.Name)]
    [JITWhenModsEnabled(ModCompatibility.SOTS.Name)]
    public class SOTSToUMTProj : GlobalProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.Vanilla;
        }

        public override void SetDefaults(Projectile entity)
        {
            if (entity.type == ModContent.ProjectileType<SOTS.Projectiles.GelAxe>())
            {
                entity.DamageType = ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }
}
