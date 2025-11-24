using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using ThrowerUnification.Core.UnitedModdedThrowerClass;
using ThrowerUnification.Content.Other;

namespace ThrowerUnification.Common.CrossmodToUMT
{
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class RogueToUMT : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.Calamity;
        }
        public override void SetDefaults(Item item)
        {
            if (item.DamageType == ModContent.GetInstance<RogueDamageClass>())
            {
                item.DamageType = (DamageClass)(object)ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }

    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class RogueToUMTProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool convertedFromRogue = false;

        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.Calamity;
        }
        public override void SetDefaults(Projectile item)
        {
            if (item.DamageType == ModContent.GetInstance<RogueDamageClass>())
            {
                item.DamageType = (DamageClass)(object)ModContent.GetInstance<UnitedModdedThrower>();

                //item.GetGlobalProjectile<ThrownToRogueVelocityFixer>().wasRogue = true;

                convertedFromRogue = true;
            }
        }
    }
}