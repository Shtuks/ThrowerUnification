using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core.UnitedModdedThrowerClass;
using ThrowerUnification.Core;

namespace ThrowerUnification.Content.StealthStrikes
{
    //Akira & Wardrobe Hummus
    [ExtendsFromMod(ModCompatibility.Calamity.Name)]
    [JITWhenModsEnabled(ModCompatibility.Calamity.Name)]
    public class UMTStealthStrikeSetup : GlobalProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.StealthStrikes;
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            // Check if the projectile belongs to your custom damage class or is a Shuriken
            if (projectile.DamageType == UnitedModdedThrower.Instance || projectile.type == ProjectileID.Shuriken)
            {
                Player player = Main.player[projectile.owner];
                CalamityPlayer calPlayer = player.GetModPlayer<CalamityPlayer>();

                if (calPlayer.StealthStrikeAvailable())
                {
                    CalamityGlobalProjectile modProj = projectile.GetGlobalProjectile<CalamityGlobalProjectile>();
                    if (modProj != null && modProj.Mod.Name != "CalamityMod" && modProj.Mod.Name != "CatalystMod" && modProj.Mod.Name != "CalamityHunt")
                    {
                        modProj.stealthStrike = true;
                    }
                }
            }
        }
    }
}
