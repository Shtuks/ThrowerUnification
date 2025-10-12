using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Common
{
    public class CustomClassTag : GlobalItem
    {
        private bool isThrower;
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Item item)
        {
            if (ThrowerModConfig.Instance.ThoriumClassTag)
            {
                if (item.DamageType == ModContent.GetInstance<UnitedModdedThrower>())
                {
                    isThrower = true;
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (isThrower)
            {
                int index = tooltips.FindIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
                if (index == -1)
                    return;
                if (ThrowerModConfig.Instance.TooltipOverride == TooltipOverrideStyle.Custom)
                {
                    tooltips.Insert(index + 1, new TooltipLine(Mod, "ThrowerTag", Language.GetTextValue("Mods.ThrowerUnification.ClassTag", ThrowerModConfig.Instance.CustomTooltipOverride))
                    {
                        OverrideColor = new Microsoft.Xna.Framework.Color(ThrowerModConfig.Instance.TagR, ThrowerModConfig.Instance.TagG, ThrowerModConfig.Instance.TagB)
                    });
                }
                else
                {
                    tooltips.Insert(index + 1, new TooltipLine(Mod, "ThrowerTag", Language.GetTextValue("Mods.ThrowerUnification.ClassTag", ThrowerModConfig.Instance.TooltipOverride))
                    {
                        OverrideColor = new Microsoft.Xna.Framework.Color(ThrowerModConfig.Instance.TagR, ThrowerModConfig.Instance.TagG, ThrowerModConfig.Instance.TagB)
                    });
                }
            }
        }
    }
}
