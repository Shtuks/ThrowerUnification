using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Common.OtherUMTToThisUMT
{
    //This class is used to convert CSE's UnitedModdedThrower damage class to the one used by Thrower Unification.
    [ExtendsFromMod(ModCompatibility.CSE.Name)]
    [JITWhenModsEnabled(ModCompatibility.CSE.Name)]
    public class CSEUMToThisUMT : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.DamageType == ModContent.GetInstance<ssm.Content.DamageClasses.UnitedModdedThrower>())
            {
                item.DamageType = (DamageClass)(object)ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }
    [ExtendsFromMod(ModCompatibility.CSE.Name)]
    [JITWhenModsEnabled(ModCompatibility.CSE.Name)]
    public class CSEUMToThisUMTProj : GlobalProjectile
    {
        public override void SetDefaults(Projectile item)
        {
            if (item.DamageType == ModContent.GetInstance<ssm.Content.DamageClasses.UnitedModdedThrower>())
            {
                item.DamageType = (DamageClass)(object)ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }
}
