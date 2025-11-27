using Terraria;
using Terraria.ModLoader;
using ThrowerUnification.Core.UnitedModdedThrowerClass;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace ThrowerUnification.Content.Other
{
    [ExtendsFromMod(ModCompatibility.Vitality.Name)]
    [JITWhenModsEnabled(ModCompatibility.Vitality.Name)]
    public class BloodHunterFix : ModSystem
    {
        private static FieldInfo canPickupField;
        private static FieldInfo holdingSpecialField;
        private static FieldInfo bloodClotDropField;

        private static FieldInfo modPlayersField;
        private static int bhIndex = -1;

        private static Type bhType;

        private const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public override void Load()
        {
            if (!ThrowerModConfig.Instance.Vitality)
                return;

            var vitality = ModLoader.GetMod("VitalityMod");
            if (vitality == null)
                return;

            bhType = vitality.Code?.GetType("VitalityMod.BloodHunter.BloodHunterPlayer");
            if (bhType == null)
                return;

            // Cache fields once
            canPickupField = bhType.GetField("canPickupBloodClots", flags);
            holdingSpecialField = bhType.GetField("holdingSpecialBloodHunterItem", flags);
            bloodClotDropField = bhType.GetField("BloodClotDrop", flags);

            // Cache reflection for modPlayers
            modPlayersField = typeof(Player).GetField("modPlayers", flags);

            // Cache index
            CacheBloodHunterIndex();
        }

        private static void CacheBloodHunterIndex()
        {
            if (modPlayersField == null || bhType == null)
                return;

            var temp = new Player();
            var mpList = modPlayersField.GetValue(temp) as IList<ModPlayer>;
            if (mpList == null)
                return;

            for (int i = 0; i < mpList.Count; i++)
            {
                if (mpList[i].GetType() == bhType)
                {
                    bhIndex = i;
                    break;
                }
            }
        }

        public override void PostUpdateEverything()
        {
            if (bhIndex == -1)
                return;

            foreach (var player in Main.player)
            {
                if (player == null || !player.active)
                    continue;

                var list = modPlayersField.GetValue(player) as IList<ModPlayer>;
                if (list == null)
                    continue;

                var bhPlayer = list[bhIndex];
                bool holdingUnited = player.HeldItem?.DamageType is UnitedModdedThrower;

                holdingSpecialField.SetValue(bhPlayer, holdingUnited);
                canPickupField.SetValue(bhPlayer, holdingUnited);
                bloodClotDropField.SetValue(bhPlayer, holdingUnited);
            }
        }
    }

    [ExtendsFromMod(ModCompatibility.Vitality.Name)]
    [JITWhenModsEnabled(ModCompatibility.Vitality.Name)]
    public class UnitedBloodHunterHits : GlobalProjectile
    {
        private static FieldInfo modPlayersField;
        private static int bhIndex = -1;

        private static FieldInfo timerF;
        private static FieldInfo dropF;
        private static FieldInfo dropChanceF;
        private static FieldInfo dropCooldownF;
        private static FieldInfo dropTimerF;
        private static FieldInfo maxBLF;
        private static FieldInfo curBLF;

        private const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public override void Load()
        {
            if (!ThrowerModConfig.Instance.Vitality)
                return;

            var vitality = ModLoader.GetMod("VitalityMod");
            if (vitality == null)
                return;

            var t = vitality.Code.GetType("VitalityMod.BloodHunter.BloodHunterPlayer");

            // Cache all fields
            timerF = t.GetField("BloodHunterTimer", flags);
            dropF = t.GetField("BloodClotDrop", flags);
            dropChanceF = t.GetField("BloodClotDropChance", flags);
            dropCooldownF = t.GetField("BloodClotDropCooldown", flags);
            dropTimerF = t.GetField("BloodClotDropTimer", flags);
            maxBLF = t.GetField("MaxBloodlust", flags);
            curBLF = t.GetField("Bloodlust", flags);

            // Cache modPlayers reference
            modPlayersField = typeof(Player).GetField("modPlayers", flags);

            CacheIndex(t);
        }

        private static void CacheIndex(Type t)
        {
            var temp = new Player();
            var list = modPlayersField.GetValue(temp) as IList<ModPlayer>;
            if (list == null)
                return;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetType() == t)
                {
                    bhIndex = i;
                    break;
                }
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (bhIndex == -1 || !(projectile.DamageType is UnitedModdedThrower))
                return;

            var player = Main.player[projectile.owner];
            if (player == null || !player.active || target == null)
                return;

            if (target.SpawnedFromStatue || target.CountsAsACritter || target.type == 488)
                return;

            var list = modPlayersField.GetValue(player) as IList<ModPlayer>;
            if (list == null)
                return;

            var bh = list[bhIndex];

            // Reset timer
            timerF.SetValue(bh, 0);

            // Grab values
            bool drop = (bool)dropF.GetValue(bh);
            float chance = (float)dropChanceF.GetValue(bh);
            int cooldown = (int)dropCooldownF.GetValue(bh);
            int timer = (int)dropTimerF.GetValue(bh);
            float maxBL = (float)maxBLF.GetValue(bh);
            float curBL = (float)curBLF.GetValue(bh);

            // Drop conditions
            if (drop && Main.rand.NextFloat(100f) <= chance && timer >= cooldown && curBL < maxBL)
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

                // Reset drop timer
                dropTimerF.SetValue(bh, 0);
            }
        }
    }
}