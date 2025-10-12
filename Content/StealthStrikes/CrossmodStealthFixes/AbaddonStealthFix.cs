using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace ThrowerUnification.Content.StealthStrikes.CrossmodStealthFixes
{
    //Akira
    [ExtendsFromMod(ModCompatibility.SacredTools.Name, ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.SacredTools.Name, ModCompatibility.Calamity.Name)]
    public class AbaddonStealthFix : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.StealthStrikes;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ModLoader.TryGetMod("SacredTools", out Mod abaddon) &&
                ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                // Try find the items
                bool isTrispear = abaddon.TryFind("Trispear", out ModItem trispearItem) && item.type == trispearItem.Type;
                bool isRogueWave = abaddon.TryFind("RogueWave", out ModItem rogueWaveItem) && item.type == rogueWaveItem.Type;

                if (isTrispear || isRogueWave)
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
