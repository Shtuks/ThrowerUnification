using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Buffs.Thrower;

namespace ThrowerUnification.Content.Buffs
{
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name, ModCompatibility.Calamity.Name)]
    [ExtendsFromMod(ModCompatibility.Thorium.Name, ModCompatibility.Calamity.Name)]
    public class ThrowingSpeedBuffAdjustment : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            if (type == ModContent.BuffType<ThrowingSpeed>())
            {
                player.GetAttackSpeed(DamageClass.Throwing) -= 0.15f;
                player.GetModPlayer<CalamityPlayer>().rogueVelocity += 0.15f;
            }
        }
    }
}
