using System;
using System.Reflection;
using CalamityMod;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.Terrarium;

namespace ThrowerUnification.Content.Other
{
    [JITWhenModsEnabled(ModCompatibility.Thorium.Name)]
    [ExtendsFromMod(ModCompatibility.Thorium.Name)]
    public class TerrariumThrowerFocusAdditions : ModSystem
    {
        private static Hook throwingEffectHook;

        public override void Load()
        {
            // Find the TerrariumHelmet.ThrowingEffect method via reflection
            MethodInfo orig = typeof(TerrariumHelmet)
                .GetMethod("ThrowingEffect", BindingFlags.Public | BindingFlags.Static);

            // Create the hook
            throwingEffectHook = new Hook(orig, (Action<Player> origMethod, Player player) =>
            {
                // Call original
                origMethod(player);

                // Inject
                if (ModCompatibility.Calamity.Loaded)
                {
                    AddCalamityStealth.AddStealth(player);
                }
            });
        }

        public override void Unload()
        {
            throwingEffectHook?.Dispose();
            throwingEffectHook = null;
        }
    }

    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    public static class AddCalamityStealth
    {
        public static void AddStealth(Player player)
        {
            var modPlayer = player.Calamity();
            modPlayer.rogueStealthMax += 1.10f;
            modPlayer.wearingRogueArmor = true;
        }
    }
}
