using Terraria.ModLoader;
using ThrowerUnification.Core;
using VitalityMod.BloodHunter;

namespace ThrowerUnification.Core.UnitedModdedThrowerClass
{
    [ExtendsFromMod(ModCompatibility.Vitality.Name)]
    [JITWhenModsEnabled(ModCompatibility.Vitality.Name)]
    public class VitalityUMT
    {
        public static StatInheritanceData SoAUMTAdd(DamageClass damageClass)
        {
            if (damageClass == ModContent.GetInstance<BloodHunterClass>())
            {
                return StatInheritanceData.Full;
            }

            return StatInheritanceData.None;
        }

        public static DamageClass GetEffectInheritance(DamageClass damageClass)
        {
            return ModContent.GetInstance<BloodHunterClass>();
        }
    }
}
