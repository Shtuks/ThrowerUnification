using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace ThrowerUnification.Content.StealthStrikes.CrossmodStealthFixes
{
    //Akira
    [ExtendsFromMod(ModCompatibility.Vitality.Name, ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Vitality.Name, ModCompatibility.Calamity.Name)]
    public class VitalityStealthFix : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.StealthStrikes;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ModLoader.TryGetMod("VitalityMod", out Mod vitality) &&
                ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                // Try find the items
                bool isThePlug = vitality.TryFind("ThePlug", out ModItem plugItem) && item.type == plugItem.Type;
                bool isTheConductor = vitality.TryFind("TheConductor", out ModItem conductorItem) && item.type == conductorItem.Type;
                bool isGluttony = vitality.TryFind("Gluttony", out ModItem gluttonyItem) && item.type == gluttonyItem.Type;

                if (isThePlug || isTheConductor || isGluttony)
                {
                    int playerIndex = player.whoAmI;

                    Main.QueueMainThreadAction(() =>
                    {
                        if (Main.player[playerIndex] != null && Main.player[playerIndex].active)
                        {
                            calamity.Call("ConsumeStealth", Main.player[playerIndex]);
                        }
                    });
                }
            }

            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
