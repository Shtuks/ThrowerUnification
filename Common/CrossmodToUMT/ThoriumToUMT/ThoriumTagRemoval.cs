using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThrowerUnification.Common.CrossmodToUMT.ThoriumToUMT
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class ThoriumTagRemoval : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.ModItem is ThoriumItem modItem)
            {
                modItem.isThrower = false;
            }
        }
    }
}
