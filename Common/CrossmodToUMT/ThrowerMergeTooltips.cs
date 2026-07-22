using System.Collections.Generic;
using System.Text.RegularExpressions;
using CalamityMod.Projectiles.Melee.Yoyos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThrowerUnification.Common.CrossmodToUMT
{
    public class ThrowerMergeTooltips : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.defense > 0 || item.accessory || item.UseSound == SoundID.Item3 || item.UseSound == SoundID.Item2)
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
                    if (!tooltips[i].Text.Contains("flamethrower")) // flamerogues be like:
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

    public class ThrowerMergeBuffTooltips : GlobalBuff
    {
        public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
        {
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
            if (!tip.Contains("flamethrower")) // flamerogues be like:
                tip = Regex.Replace(tip, "thrower", val2);
            tip = Regex.Replace(tip, "throwing", val2); //some mods say "throwing" instead of thrower.
            tip = Regex.Replace(tip, "kinetic", val2);
            tip = Regex.Replace(tip, "malevolent", val2);
            tip = Regex.Replace(tip, "rogue", val2);
                    
            tip = Regex.Replace(tip, "Thrower", val1);
            tip = Regex.Replace(tip, "Throwing", val1);
            tip = Regex.Replace(tip, "Kinetic", val1);
            tip = Regex.Replace(tip, "Rogue", val1);
            tip = Regex.Replace(tip, "Malevolent", val1);
        }
    }
}
