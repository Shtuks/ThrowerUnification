using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ModLoader;

namespace ThrowerUnification.Common.CrossmodToUMT
{
    public class ThrowerMergeTooltips : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.defense > 0 || item.accessory)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (i == 0) continue;

                    string val1 = "Thrower";
                    string val2 = "thrower";

                    if (ThrowerModConfig.Instance.TooltipOverride == TooltipOverrideStyle.Thrower)
                    {
                        val1 = "Thrower";
                        val2 = "thrower";
                    }
                    if (ThrowerModConfig.Instance.TooltipOverride == TooltipOverrideStyle.Kinetic)
                    {
                        val1 = "Kinetic";
                        val2 = "kinetic";
                    }
                    if (ThrowerModConfig.Instance.TooltipOverride == TooltipOverrideStyle.Malevolent)
                    {
                        val1 = "Malevolent";
                        val2 = "malevolent";
                    }
                    if (ThrowerModConfig.Instance.TooltipOverride == TooltipOverrideStyle.Rogue)
                    {
                        val1 = "Rogue";
                        val2 = "rogue";
                    }
                    if (ThrowerModConfig.Instance.TooltipOverride == TooltipOverrideStyle.Custom)
                    {
                        val1 = ThrowerModConfig.Instance.CustomTooltipOverride;
                        val2 = ThrowerModConfig.Instance.CustomTooltipOverride.ToLower();
                    }

                    //This only works for english.... is there a way around this maybe?
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "thrower", val2);
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "throwing", val2); //some mods say "throwing" instead of thrower.
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "kinetic", val2);
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "malevolent", val2);
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "rogue", val2);

                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "Thrower", val1);
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "Throwing", val1);
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "Kinetic", val1);
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "Rogue", val1);
                    tooltips[i].Text = Regex.Replace(tooltips[i].Text, "Malevolent", val1);
                }
            }
        }
    }
}
