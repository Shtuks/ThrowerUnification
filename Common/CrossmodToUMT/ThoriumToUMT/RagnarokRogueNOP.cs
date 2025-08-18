using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Terraria.ModLoader;
using ThrowerUnification.Core;
using Terraria;

namespace ThrowerUnification.Common.CrossmodToUMT.ThoriumToUMT
{
    [ExtendsFromMod(ModCompatibility.Ragnarok.Name)]
    [JITWhenModsEnabled(ModCompatibility.Ragnarok.Name)]
    public class RagnarokRogueNOP : ModSystem
    {
        //Yeah i don't know what the FUCK Ragnarok was thinking making their own damage type, but somehow, even after loading after them, it STILL overrides ours. So I'm just killing the code :)
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ThrowerModConfig.Instance.Vanilla;
        }

        private Hook _hApplies;
        private Hook _hSetDefaults;

        public override void Load()
        {
            if (!ModLoader.TryGetMod("RagnarokMod", out var ragnarok) || ragnarok?.Code == null)
                return;

            var t = ragnarok.Code.GetTypes()
                .FirstOrDefault(x => x.Name == "ChangeThrowerToRogue" && typeof(GlobalItem).IsAssignableFrom(x));
            if (t == null) return;

            var miApplies = t.GetMethod("AppliesToEntity",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                types: new[] { typeof(Item), typeof(bool) },
                modifiers: null);

            var miSetDefaults = t.GetMethod("SetDefaults",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                types: new[] { typeof(Item) },
                modifiers: null);

            if (miApplies != null)
                _hApplies = new Hook(miApplies, (AppliesToEntity_Rep)AppliesToEntity_Hook);

            if (miSetDefaults != null)
                _hSetDefaults = new Hook(miSetDefaults, (SetDefaults_Rep)SetDefaults_Hook);
        }

        public override void Unload()
        {
            _hApplies?.Dispose(); _hApplies = null;
            _hSetDefaults?.Dispose(); _hSetDefaults = null;
        }

        // Delegates must match: (orig, self, args...)
        private delegate bool AppliesToEntity_Orig(object self, Item item, bool lateInstantiation);
        private delegate bool AppliesToEntity_Rep(AppliesToEntity_Orig orig, object self, Item item, bool lateInstantiation);
        private static bool AppliesToEntity_Hook(AppliesToEntity_Orig orig, object self, Item item, bool lateInstantiation) => false;

        private delegate void SetDefaults_Orig(object self, Item item);
        private delegate void SetDefaults_Rep(SetDefaults_Orig orig, object self, Item item);
        private static void SetDefaults_Hook(SetDefaults_Orig orig, object self, Item item) { /* no-op */ }
    }
}
