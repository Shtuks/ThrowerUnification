using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfernalEclipseAPI.Core.DamageClasses.MergedRogueClass;
using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Common.OtherUMTToThisUMT
{
    //This class is used to convert Infernal Eclipse's MergedThrowerRogue damage class to the one used by Thrower Unification.
    [ExtendsFromMod(ModCompatibility.InfernalEclipse.Name)]
    [JITWhenModsEnabled(ModCompatibility.InfernalEclipse.Name)]
    public class InfernalMergedThrowerRogueToUMT : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.DamageType == ModContent.GetInstance<MergedThrowerRogue>())
            {
                item.DamageType = (DamageClass)(object)ModContent.GetInstance<UnitedModdedThrower>();
            }
        }
    }

    [ExtendsFromMod(ModCompatibility.InfernalEclipse.Name)]
    [JITWhenModsEnabled(ModCompatibility.InfernalEclipse.Name)]
    public class InfernalMergedThrowerRogueToUMTProj : GlobalProjectile
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
