using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using ThrowerUnification.Core.UnitedModdedThrowerClass;
using ThrowerUnification.Core;

namespace ThrowerUnification.Content.CrossmodDamageFix
{
    public class BloodHunterLexiconFix : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.StealthStrikes;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
                                   Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ModLoader.TryGetMod("VitalityMod", out Mod vitality) &&
                vitality.TryFind("BloodHunterLexicon", out ModItem lexicon) &&
                item.type == lexicon.Type)
            {
                // Delay marking the projectile until it spawns
                Main.QueueMainThreadAction(() =>
                {
                    foreach (Projectile proj in Main.projectile)
                    {
                        if (proj.active && proj.owner == player.whoAmI && proj.type == type)
                        {
                            proj.DamageType = UnitedModdedThrower.Instance;
                        }
                    }
                });
            }

            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
