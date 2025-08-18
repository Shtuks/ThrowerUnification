using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThrowerUnification.Common.CrossmodToUMT.ThoriumToUMT
{
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    public class ThoriumTagRemoval : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.Vanilla;
        }

        public ThoriumItem getThoriumItem(Item item)
        {
            return item.ModItem is ThoriumItem modItem ? modItem : null;
        }

        public override void SetDefaults(Item item)
        {
            ThoriumItem thoriumItem = getThoriumItem(item);
            if (thoriumItem == null) return;
            thoriumItem.isThrower = false;
        }
    }
}
