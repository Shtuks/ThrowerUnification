﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ThrowerUnification.Content.StealthStrikes.CrossmodStealthFixes
{
    //Akira
    [ExtendsFromMod(ModCompatibility.ThoriumRework.Name, ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.ThoriumRework.Name, ModCompatibility.Calamity.Name)]
    public class GraniteEnergyStormStealthFix : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.StealthStrikes;
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ModLoader.TryGetMod("ThoriumRework", out Mod thorRework) &&
                thorRework.TryFind("PocketEnergyStorm", out ModItem pes) &&
                item.type == pes.Type &&
                ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                // Delay the stealth consumption by one tick to allow stealth strike logic to complete
                int playerIndex = player.whoAmI;

                // Queue on the next update tick
                Main.QueueMainThreadAction(() =>
                {
                    if (Main.player[playerIndex] != null && Main.player[playerIndex].active)
                    {
                        calamity.Call("ConsumeStealth", Main.player[playerIndex]);
                    }
                });
            }

            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
