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
using CalamityMod;

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
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            if (!projectile.friendly || projectile.hostile || projectile.npcProj || projectile.trap)
                return;

            bool isUMT =
                projectile.DamageType == UnitedModdedThrower.Instance ||
                projectile.type == ProjectileID.Shuriken;

            if (!isUMT)
                return;

            Player player = Main.player[projectile.owner];
            if (!player.active)
                return;

            CalamityPlayer calPlayer = player.GetModPlayer<CalamityPlayer>();

            if (!calPlayer.StealthStrikeAvailable())
                return;

            string projModName = projectile.ModProjectile?.Mod?.Name;

            bool fromCalamityFamily =
                projModName == "CalamityMod" ||
                projModName == "CatalystMod" ||
                projModName == "CalamityHunt" ||
                projModName == "Clamity";

            if (fromCalamityFamily)
                return;

            projectile.Calamity().stealthStrike = true;
        }
    }
}
