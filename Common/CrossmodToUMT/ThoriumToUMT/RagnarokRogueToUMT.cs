using RagnarokMod.Utils;
using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Common.CrossmodToUMT.ThoriumToUMT
{
    [ExtendsFromMod(ModCompatibility.Ragnarok.Name)]
    [JITWhenModsEnabled(ModCompatibility.Ragnarok.Name)]
    public class RagnarokToUMT : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.Vanilla;
        }

        public override void SetDefaults(Item item)
        {
            if (item.DamageType == ModContent.GetInstance<ThoriumRogueClass>())
            {
                item.DamageType = (DamageClass)(object)ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }
    [ExtendsFromMod(ModCompatibility.Ragnarok.Name)]
    [JITWhenModsEnabled(ModCompatibility.Ragnarok.Name)]
    public class RagnarokToUMTProj : GlobalProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.Vanilla;
        }
        public override void SetDefaults(Projectile item)
        {
            if (item.DamageType == ModContent.GetInstance<ThoriumRogueClass>())
            {
                item.DamageType = (DamageClass)(object)ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }
}
