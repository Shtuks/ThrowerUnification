using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core;
using ThrowerUnification.Core.UnitedModdedThrowerClass;
using WHummusMultiModBalancing;

namespace ThrowerUnification.Common.OtherUMTToThisUMT
{
    //This class is used to convert WHummus' MergedThrowerRogue damage class to the one used by Thrower Unification.
    [ExtendsFromMod(ModCompatibility.WHummusMultiModBalancing.Name)]
    [JITWhenModsEnabled(ModCompatibility.WHummusMultiModBalancing.Name)]
    public class WHummusMergedThrowerRogueToUMT : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.DamageType == ModContent.GetInstance<MergedThrowerRogue>())
            {
                item.DamageType = (DamageClass)(object)ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }

    [ExtendsFromMod(ModCompatibility.WHummusMultiModBalancing.Name)]
    [JITWhenModsEnabled(ModCompatibility.WHummusMultiModBalancing.Name)]
    public class WHummusMergedThrowerRogueToUMTProj : GlobalProjectile
    {
        public override void SetDefaults(Projectile item)
        {
            if (item.DamageType == ModContent.GetInstance<MergedThrowerRogue>())
            {
                item.DamageType = (DamageClass)(object)ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }
}
