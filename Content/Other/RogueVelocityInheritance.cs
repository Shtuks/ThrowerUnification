using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ThrowerUnification.Content.Other
{
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class RogueVelocityInheritance : ModPlayer
    {
        public override bool IsLoadingEnabled(Mod mod) => false; //ModCompatibility.Calamity.Loaded
        public override void PostUpdateRunSpeeds()
        {
            // verify calamity exists
            if (!ModLoader.TryGetMod("CalamityMod", out Mod calamity))
                return;

            // vanilla thrown velocity multiplier
            float thrownVelo = Player.ThrownVelocity;

            if (thrownVelo != 0f)
            {
                // That means we want to ADD rogueVelocity * thrownVelo so that rogueVelocity functions as a merged stat
                object getResult = calamity.Call("GetRogueVelocity", Player);

                if (getResult is float rogueVelo)
                {
                    float addAmount = (thrownVelo - 1f);

                    // Official safe API call
                    calamity.Call("AddRogueVelocity", Player, addAmount);
                }
            }
        }
    }


    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class ThrownToRogueVelocityFixer : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override bool IsLoadingEnabled(Mod mod) => false; //ThrowerModConfig.Instance.Calamity && ModCompatibility.Calamity.Loaded);

        // Track if projectile was originally Rogue
        public bool wasRogue = false;

        public override void SetDefaults(Projectile proj)
        {
            if (proj.DamageType == ModContent.GetInstance<RogueDamageClass>())
                wasRogue = true;
        }

        public override void OnSpawn(Projectile proj, Terraria.DataStructures.IEntitySource source)
        {
            Player owner = Main.player[proj.owner];
            float thrownVelo = owner.ThrownVelocity;

            //Now work off of just RogueVelocity even if not rogue cause that now works as the merged velocity stat (having Calamity not enabled will just make everything run as normal)
            if (!wasRogue)
            {
                proj.velocity /= thrownVelo;

                /*
                if (proj.velocity != Vector2.Zero && thrownVelo != 1f)
                {
                    // Desired velocity based on rogueVelo
                    object getResult = ModLoader.GetMod("CalamityMod")?.Call("GetRogueVelocity", owner);
                    if (getResult is float rogueVelo)
                    {
                        Vector2 desiredVelocity = proj.velocity * rogueVelo;

                        // Only boost if current velocity is too low
                        if ((proj.velocity * thrownVelo).Length() < desiredVelocity.Length())
                        {
                            proj.velocity = desiredVelocity;
                        }
                    }
                }
                */

                return;
            }

            //If originally rogue, now fixed since it originally used JUST rogueVelocity +thrownVelocity flatly (for some reason)
            if (thrownVelo > 1f && proj.velocity != Vector2.Zero)
            {
                // Subtract a flat amount in the direction of the projectile
                Vector2 flatSubtraction = Vector2.Normalize(proj.velocity) * (thrownVelo);
                proj.velocity -= flatSubtraction;
            }
        }
    }
}
