using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Core.Players
{
    //Akira
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class ThrowerPlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            Player player = Main.LocalPlayer;
            var CalPlayer = player.GetModPlayer<CalamityPlayer>();

            Player.GetDamage<UnitedModdedThrower>() += CalPlayer.stealthDamage;
        }
    }
}
