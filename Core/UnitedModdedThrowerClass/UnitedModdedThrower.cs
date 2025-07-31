using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Steamworks;
using Terraria;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles;
using Terraria.DataStructures;
using Terraria.ID;

namespace ThrowerUnification.Core.UnitedModdedThrowerClass
{
    public partial class UnitedModdedThrower : DamageClass, ColoredDamageTypesSupport.IDamageColor
    {

        internal static UnitedModdedThrower Instance;
        Color ColoredDamageTypesSupport.IDamageColor.DamageColor => new Color(ThrowerModConfig.Instance.R, ThrowerModConfig.Instance.G, ThrowerModConfig.Instance.B);

        public override void Load()
        {
            Instance = this;
        }

        public override void Unload()
        {
            Instance = null;
        }

        public override bool GetPrefixInheritance(DamageClass damageClass)
        {
            return damageClass == Ranged;
        }
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Throwing || damageClass == Generic)
            {
                return StatInheritanceData.Full;
            }
            if (ModCompatibility.SacredTools.Loaded)
            {
                return SoAUMT.SoAUMTAdd(damageClass);
            }
            if (ModCompatibility.Vitality.Loaded)
            {
                return VitalityUMT.SoAUMTAdd(damageClass);
            }

            return StatInheritanceData.None;
        }

        private LocalizedText GetName()
        {
            string key = $"Mods.ThrowerUnification.DamageClasses.UnitedModdedThrower.DisplayName.{ThrowerModConfig.Instance.TooltipOverride}";
            return Language.GetText(key);
        }
        public override LocalizedText DisplayName => GetName();
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if(damageClass == Throwing)
            {
                return true;
            }

            if (ModCompatibility.SacredTools.Loaded)
            {
                if (damageClass == SoAUMT.GetEffectInheritance(damageClass))
                {
                    return true;
                }
            }

            if (ModCompatibility.Calamity.Loaded)
            {
                if (damageClass == CalUMT.GetEffectInheritance(damageClass))
                {
                    return true;
                }
            }

            if (ModCompatibility.Vitality.Loaded)
            {
                if (damageClass == VitalityUMT.GetEffectInheritance(damageClass))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
