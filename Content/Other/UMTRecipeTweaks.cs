using Terraria;
using Terraria.ModLoader;

namespace ThrowerUnification.Content.Other
{
    public class UMTRecipeTweaks : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int index = 0; index < Recipe.numRecipes; ++index)
            {
                Recipe recipe = Main.recipe[index];

                if (ModCompatibility.Thorium.Loaded && ModCompatibility.Calamity.Loaded && ThrowerModConfig.Instance.StealthStrikes)
                {
                    if (recipe.HasResult(ModCompatibility.Calamity.Mod.Find<ModItem>("ScarletDevil")))
                    {
                        recipe.AddIngredient(ModCompatibility.Thorium.Mod.Find<ModItem>("AngelsEnd"));
                    }
                }
            }
        }
    }
}
