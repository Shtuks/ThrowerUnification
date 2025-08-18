using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RagnarokMod.Utils;
using SacredTools.Content.DamageClasses;
using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core;
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
