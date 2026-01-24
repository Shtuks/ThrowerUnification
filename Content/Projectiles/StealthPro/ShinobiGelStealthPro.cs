using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core.UnitedModdedThrowerClass;

namespace ThrowerUnification.Content.Projectiles.StealthPro
{
    //Wardrobe Hummus
    public class ShinobiGelStealthPro : ModProjectile
    {
        private bool? useCyanDust;
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SpikedSlimeSpike}";
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = ModContent.GetInstance<UnitedModdedThrower>();
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            NPC target = null;
            float closestDist = 700f;

            // Find nearest valid NPC
            for (int i = 0; i < Main.maxNPCs; ++i)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(null, false) && !npc.friendly)
                {
                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        target = npc;
                    }
                }
            }

            // Homing logic
            if (target != null)
            {
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();
                float speed = 12f;
                Projectile.velocity = (Projectile.velocity * 20f + direction * speed) / 21f;
            }
            else
            {
                Projectile.velocity *= 0.98f;
            }

            // ALWAYS rotate to match velocity
            if (Projectile.velocity.LengthSquared() > 0.001f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }

            // Dust
            if (Main.rand.NextBool(4))
            {
                int dustIndex = Dust.NewDust(
                    ((Entity)((ModProjectile)this).Projectile).position, 
                    ((Entity)((ModProjectile)this).Projectile).width, 
                    ((Entity)((ModProjectile)this).Projectile).height, 
                    154, 
                    (float)Main.rand.Next(-5, 5), 
                    (float)Main.rand.Next(-5, 5), 
                    50, 
                    default(Color), 
                    1.5f);

                Dust dust = Main.dust[dustIndex];
                dust.velocity *= 0.3f;
                dust.noGravity = true;
            }

            // Kill if fully stopped and no homing target
            if (target == null && Projectile.velocity.LengthSquared() < 0.05f)
            {
                Projectile.Kill();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Bounce off X
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X * 0.8f; // energy loss

            // Bounce off Y
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y * 0.8f;

            // Small velocity clamp to prevent infinite micro-bouncing
            if (Math.Abs(Projectile.velocity.X) < 0.2f)
                Projectile.velocity.X = 0f;

            if (Math.Abs(Projectile.velocity.Y) < 0.2f)
                Projectile.velocity.Y = 0f;

            return false;
        }

    }
}
