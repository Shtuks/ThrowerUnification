using CalamityMod;
using Terraria.ModLoader;
using ThrowerUnification.Core;

namespace ThrowerUnification.Core.UnitedModdedThrowerClass
{
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class CalUMT
    {
        public static StatInheritanceData CalUMTAdd(DamageClass damageClass)
        {
            if (damageClass == ModContent.GetInstance<RogueDamageClass>())
            {
                return StatInheritanceData.Full;
            }

            return StatInheritanceData.None;
        }

        public static DamageClass GetEffectInheritance(DamageClass damageClass)
        {
            return ModContent.GetInstance<RogueDamageClass>();
        }
    }
}
