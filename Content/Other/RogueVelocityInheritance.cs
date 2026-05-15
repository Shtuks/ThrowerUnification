using Terraria;
using Terraria.ModLoader;
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
        public override bool IsLoadingEnabled(Mod mod) => ThrowerModConfig.Instance.Calamity && ModCompatibility.Calamity.Loaded;

        public bool wasRogue;

        public override void OnSpawn(Projectile proj, Terraria.DataStructures.IEntitySource source)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            if (proj.owner < 0 || proj.owner >= Main.maxPlayers)
                return;

            Player owner = Main.player[proj.owner];

            if (!owner.active || !proj.friendly)
                return;

            if (!wasRogue)
                return;

            float thrownVelo = owner.ThrownVelocity;

            if (thrownVelo <= 0f)
                return;

            //scale down initial velocity so rogue-velocity later multiplies correctly
            proj.velocity /= thrownVelo;
            proj.netUpdate = true;
        }
    }
}
