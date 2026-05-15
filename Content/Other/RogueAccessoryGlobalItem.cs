using CalamityMod;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ModLoader;

namespace ThrowerUnification.Content.Other
{
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class RogueAccessoryGlobalItem : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod) => ThrowerModConfig.Instance.Calamity && ModCompatibility.Calamity.Loaded;
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ModContent.ItemType<GloveOfPrecision>())
            {
                player.GetDamage<RogueDamageClass>() -= 0.1f;
                player.GetDamage(DamageClass.Throwing) += 0.1f;

                player.GetCritChance<RogueDamageClass>() -= 10;
                player.GetCritChance(DamageClass.Throwing) += 10;
            }

            if (item.type == ModContent.ItemType<GloveOfRecklessness>())
            {
                player.GetDamage<RogueDamageClass>() += 0.1f;
                player.GetDamage(DamageClass.Throwing) -= 0.1f;

                player.GetCritChance<RogueDamageClass>() += 5;
                player.GetCritChance(DamageClass.Throwing) -= 5;

                player.GetAttackSpeed<RogueDamageClass>() -= 0.15f;
                player.GetAttackSpeed(DamageClass.Throwing) += 0.15f;
            }
        }
    }
}
