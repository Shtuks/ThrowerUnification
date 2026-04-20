using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
namespace ThrowerUnification.Content.UI
{
    public class BalancingNotification : IInGameNotification
    {
        public bool ShouldBeRemoved => timeLeft <= 0;

        private int timeLeft = 5 * 60;
       
        private float Scale
        {
            get
            {
                if (timeLeft < 30)
                {
                    return MathHelper.Lerp(0f, 1f, timeLeft / 30f);
                }

                if (timeLeft > 285)
                {
                    return MathHelper.Lerp(1f, 0f, (timeLeft - 285) / 15f);
                }

                return 1f;
            }
        }

        private float Opacity
        {
            get
            {
                if (Scale <= 0.5f)
                {
                    return 0f;
                }

                return (Scale - 0.5f) / 0.5f;
            }
        }

        public void Update()
        {
            if (timeLeft <= 30 || timeLeft > 200)
                timeLeft--;

            if (timeLeft < 0)
            {
                timeLeft = 0;

            }
        }

        public void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition)
        {
            if (Opacity <= 0f)
            {
                return;
            }

            string title;

            if (ModLoader.HasMod("InfernumMode"))
                title = Language.GetTextValue("Mods.ThrowerUnification.UI.IEoRNotice");
            else
                title = Language.GetTextValue("Mods.ThrowerUnification.UI.HummusNotice");

            float effectiveScale = Scale * 1.1f;
            Vector2 size = (FontAssets.ItemStack.Value.MeasureString(title) + new Vector2(58f, 100f)) * effectiveScale;
            Rectangle panelSize = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - size.Y) * 0.5f), size);

            // Check if the mouse is hovering over the notification.
            bool hovering = panelSize.Contains(Main.MouseScreen.ToPoint());

            Utils.DrawInvBG(spriteBatch, panelSize, new Color(64, 109, 164) * (hovering ? 0.75f : 0.5f));
            Vector2 vector = panelSize.Right();
            Utils.DrawBorderString(color: new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor / 5, Main.mouseTextColor) * Opacity, sb: spriteBatch, text: title, pos: vector - Vector2.UnitX * 10f, scale: effectiveScale * 0.9f, anchorx: 1f, anchory: 0.4f);

            if (hovering)
            {
                OnMouseOver();
            }
        }

        private void OnMouseOver()
        {
            if (PlayerInput.IgnoreMouseInterface)
            {
                return;
            }

            Main.LocalPlayer.mouseInterface = true;

            if (!Main.mouseLeft || !Main.mouseLeftRelease)
            {
                return;
            }

            Main.mouseLeftRelease = false;

            if (timeLeft > 30 && timeLeft < 255)
            {
                timeLeft = 30;
            }
        }

        public void PushAnchor(ref Vector2 positionAnchorBottom)
        {
            positionAnchorBottom.Y -= 180f * Opacity;
        }
    }
}
