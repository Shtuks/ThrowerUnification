using Terraria.ModLoader;
using VitalityMod.BloodHunter;

namespace ThrowerUnification
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
