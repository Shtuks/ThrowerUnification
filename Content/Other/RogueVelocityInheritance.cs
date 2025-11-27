using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria.ID;

namespace ThrowerUnification.Content.Other
{
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class RogueVelocityInheritance : ModPlayer
    {
        private static Mod calamity;
        private static bool apiReady = false;

        public override bool IsLoadingEnabled(Mod mod) =>
            ThrowerModConfig.Instance.Calamity && ModCompatibility.Calamity.Loaded;

        public override void Load()
        {
            // Cache mod once
            if (ModLoader.TryGetMod("CalamityMod", out calamity))
                apiReady = true;
        }

        public override void PostUpdateRunSpeeds()
        {
            if (!apiReady)
                return;

            float thrownVelo = Player.ThrownVelocity;
            if (thrownVelo <= 1f)
                return;

            // Query Calamity safely
            object result = calamity.Call("GetRogueVelocity", Player);
            if (result is float rogueVelo)
            {
                float addAmount = thrownVelo - 1f;
                calamity.Call("AddRogueVelocity", Player, addAmount);
            }
        }
    }

    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class ThrownToRogueVelocityFixer : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool IsLoadingEnabled(Mod mod) =>
            ThrowerModConfig.Instance.Calamity && ModCompatibility.Calamity.Loaded;

        // Deterministic flag — no need to manually sync
        public bool wasRogue;

        public override void OnSpawn(Projectile proj, Terraria.DataStructures.IEntitySource source)
        {
            // Only modify velocity on the SERVER to avoid desync
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            // Validate owner safely
            if (proj.owner < 0 || proj.owner >= Main.maxPlayers)
                return;

            Player owner = Main.player[proj.owner];
            if (owner == null || !owner.active)
                return;

            float thrownVelo = owner.ThrownVelocity;

            // Prevent division by zero or unstable behaviour
            if (thrownVelo <= 0f)
                return;

            // NON-ROGUE PROJECTILE: scale down initial velocity so rogue-velocity later multiplies correctly
            if (!wasRogue)
            {
                proj.velocity /= thrownVelo;
                proj.netUpdate = true;
                return;
            }

            /*
            // ROGUE PROJECTILE – remove calamity’s extra flat thrown velocity boost
            if (thrownVelo > 1f && proj.velocity != Vector2.Zero)
            {
                Vector2 flat = Vector2.Normalize(proj.velocity) * thrownVelo;
                proj.velocity -= flat;
                proj.netUpdate = true;
            }
            */
        }
    }
}
