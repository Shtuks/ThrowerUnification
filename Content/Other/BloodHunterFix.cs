using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core.UnitedModdedThrowerClass;
using System.Reflection;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace ThrowerUnification.Content.Other
{
    [ExtendsFromMod("VitalityMod")]
    public class BloodHunterFix : ModSystem
    {
        private FieldInfo canPickupField;
        private FieldInfo holdingSpecialField;
        private FieldInfo bloodClotDropField; // renamed for clarity
        private string bloodHunterPlayerFullName = "VitalityMod.BloodHunter.BloodHunterPlayer";

        public override void Load()
        {
            var vitalityMod = ModLoader.GetMod("VitalityMod");
            if (vitalityMod == null || !ThrowerModConfig.Instance.Vitality)
                return;

            var bhType = vitalityMod.Code?.GetType(bloodHunterPlayerFullName);
            if (bhType == null)
                return;

            // Reflect the fields in BloodHunterPlayer
            canPickupField = bhType.GetField("canPickupBloodClots", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            holdingSpecialField = bhType.GetField("holdingSpecialBloodHunterItem", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            bloodClotDropField = bhType.GetField("BloodClotDrop", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public override void PostUpdateEverything()
        {
            if (canPickupField == null || holdingSpecialField == null || bloodClotDropField == null)
                return;

            foreach (var player in Main.player)
            {
                if (player == null || !player.active)
                    continue;

                var modPlayersField = typeof(Player).GetField("modPlayers", BindingFlags.Instance | BindingFlags.NonPublic);
                var modPlayersList = modPlayersField?.GetValue(player) as IList<ModPlayer>;
                if (modPlayersList == null)
                    continue;

                object bhPlayer = null;
                foreach (var mp in modPlayersList)
                {
                    if (mp.GetType().FullName == bloodHunterPlayerFullName)
                    {
                        bhPlayer = mp;
                        break;
                    }
                }

                if (bhPlayer == null)
                    continue;

                bool holdingUnitedThrower = player.HeldItem?.DamageType is UnitedModdedThrower;

                // Force the BloodHunter fields
                holdingSpecialField.SetValue(bhPlayer, holdingUnitedThrower);
                canPickupField.SetValue(bhPlayer, holdingUnitedThrower);
                bloodClotDropField.SetValue(bhPlayer, holdingUnitedThrower);
            }
        }
    }

    [ExtendsFromMod("VitalityMod")]
    public class UnitedBloodHunterHits : GlobalProjectile
    {
        public override void Load()
        {
            var vitalityMod = ModLoader.GetMod("VitalityMod");
            if (vitalityMod == null || !ThrowerModConfig.Instance.Vitality)
                return;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!(projectile?.DamageType is UnitedModdedThrower))
                return;

            var player = Main.player[projectile.owner];
            if (player == null || !player.active || target == null || target.SpawnedFromStatue || target.CountsAsACritter || target.type == 488)
                return;

            var modPlayersField = typeof(Player).GetField("modPlayers", BindingFlags.Instance | BindingFlags.NonPublic);
            var modPlayersList = modPlayersField?.GetValue(player) as IList<ModPlayer>;
            if (modPlayersList == null || modPlayersList.Count == 0)
                return;

            object bhPlayer = null;
            foreach (var mp in modPlayersList)
            {
                if (mp?.GetType()?.FullName == "VitalityMod.BloodHunter.BloodHunterPlayer")
                {
                    bhPlayer = mp;
                    break;
                }
            }

            if (bhPlayer == null)
                return;

            // Cache fields safely
            var type = bhPlayer.GetType();
            var bloodHunterTimer = type.GetField("BloodHunterTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var bloodClotDrop = type.GetField("BloodClotDrop", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var bloodClotDropChance = type.GetField("BloodClotDropChance", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var bloodClotDropCooldown = type.GetField("BloodClotDropCooldown", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var bloodClotDropTimer = type.GetField("BloodClotDropTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var maxBloodlust = type.GetField("MaxBloodlust", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var currentBloodlust = type.GetField("Bloodlust", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (bloodHunterTimer != null)
                bloodHunterTimer.SetValue(bhPlayer, 0);

            if (bloodClotDrop == null || bloodClotDropChance == null || bloodClotDropCooldown == null || bloodClotDropTimer == null || maxBloodlust == null || currentBloodlust == null)
                return;

            bool dropClot = (bool)bloodClotDrop.GetValue(bhPlayer);
            float dropChance = (float)bloodClotDropChance.GetValue(bhPlayer);
            int dropCooldown = (int)bloodClotDropCooldown.GetValue(bhPlayer);
            int dropTimer = (int)bloodClotDropTimer.GetValue(bhPlayer);
            float maxBL = (float)maxBloodlust.GetValue(bhPlayer);
            float curBL = (float)currentBloodlust.GetValue(bhPlayer);

            if (dropClot && Main.rand.NextFloat(0, 100) <= dropChance && dropTimer >= dropCooldown && curBL < maxBL)
            {
                int projType = ModContent.ProjectileType<VitalityMod.BloodHunter.BloodStream>();
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    target.Center,
                    Vector2.Zero,
                    projType,
                    0,
                    0f,
                    player.whoAmI
                );

                bloodClotDropTimer.SetValue(bhPlayer, 0);
            }
        }
    }
}
