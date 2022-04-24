using System;
using System.Diagnostics.Eventing.Reader;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Musician.Core.Models;
using Nekres.Musician.UI.Models;

namespace Nekres.Musician.Controls {
    internal class SheetButton : DetailsButton
    {
        public event EventHandler<EventArgs> OnPracticeClick;
        public event EventHandler<EventArgs> OnEmulateClick;
        public event EventHandler<ValueEventArgs<bool>> OnPreviewClick;

        private const int SHEETBUTTON_WIDTH = 327;
        private const int SHEETBUTTON_HEIGHT = 100;
        private const int USER_WIDTH = 75;
        private const int BOTTOMSECTION_HEIGHT = 35;

        #region Textures

        private static Texture2D _beatManiaSprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("beatmania.png");
        private static Texture2D _glowBeatManiaSprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_beatmania.png");
        private static Texture2D _autoplaySprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("autoplay.png");
        private static Texture2D _glowAutoplaySprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_autoplay.png");
        private static Texture2D _playSprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("stop.png");
        private static Texture2D _glowPlaySprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_stop.png");
        private static Texture2D _stopSprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("play.png");
        private static Texture2D _glowStopSprite = MusicianModule.ModuleInstance.ContentsManager.GetTexture("glow_play.png");
        private static Texture2D _dividerSprite = GameService.Content.GetTexture("157218");
        private static Texture2D _iconBoxSprite = GameService.Content.GetTexture("controls/detailsbutton/605003");

        #endregion

        public readonly Guid Id;

        private string _artist;
        public string Artist { 
            get => _artist; 
            set => SetProperty(ref _artist, value);
        }

        private string _user;
        public string User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private bool _isPreviewing;

        private Rectangle _practiceButtonBounds;
        private bool _mouseOverPractice;
        private Rectangle _emulateButtonBounds;
        private bool _mouseOverEmulate;
        private Rectangle _previewButtonBounds;
        private bool _mouseOverPreview;

        public SheetButton(MusicSheetModel sheet)
        {
            Id = sheet.Id;
            Artist = sheet.Artist;
            User = sheet.User;
            Title = sheet.Title;
            Icon = sheet.Instrument.GetIcon();
            Size = new Point(SHEETBUTTON_WIDTH, SHEETBUTTON_HEIGHT);
        }

        protected override void OnMouseLeft(MouseEventArgs e)
        {
            _mouseOverPractice = false;
            _mouseOverEmulate = false;
            base.OnMouseLeft(e);
        }

        protected override void OnMouseMoved(MouseEventArgs e)
        {
            var relPos = RelativeMousePosition;
            _mouseOverPractice = _practiceButtonBounds.Contains(relPos);
            _mouseOverEmulate = _emulateButtonBounds.Contains(relPos);
            _mouseOverPreview = _previewButtonBounds.Contains(relPos);

            if (_mouseOverPractice)
                BasicTooltipText = "Practice mode (Synthesia)";
            else if (_mouseOverEmulate)
                BasicTooltipText = "Emulate keys (Autoplay)";
            else if (_mouseOverPreview)
                BasicTooltipText = "Preview";
            else
                BasicTooltipText = Title;
            base.OnMouseMoved(e);
        }

        protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
        {
            if (_mouseOverPractice)
                OnPracticeClick?.Invoke(this, EventArgs.Empty);
            else if (_mouseOverEmulate)
                OnEmulateClick?.Invoke(this, EventArgs.Empty);
            else if (_mouseOverPreview)
            {
                _isPreviewing = !_isPreviewing;
                OnPreviewClick?.Invoke(this, new ValueEventArgs<bool>(_isPreviewing));
            }

            base.OnLeftMouseButtonReleased(e);
        }

        protected override CaptureType CapturesInput() {
            return CaptureType.Mouse | CaptureType.Filter;
        }

        public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
        {
            var iconSize = IconSize == DetailsIconSize.Large ? SHEETBUTTON_HEIGHT : SHEETBUTTON_HEIGHT - BOTTOMSECTION_HEIGHT;

            // Draw background
            spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Color.Black * 0.25f);

            // Draw bottom section (overlap to make background darker here)
            spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, bounds.Height - BOTTOMSECTION_HEIGHT, bounds.Width - BOTTOMSECTION_HEIGHT, BOTTOMSECTION_HEIGHT), Color.Black * 0.1f);

            // Draw preview icon
            _previewButtonBounds = new Rectangle(SHEETBUTTON_WIDTH - 36, bounds.Height - BOTTOMSECTION_HEIGHT + 1, 32, 32);
            if (_mouseOverPreview)
            {
                if (_isPreviewing)
                    spriteBatch.DrawOnCtrl(this, _glowStopSprite, _previewButtonBounds, Color.White);
                else
                    spriteBatch.DrawOnCtrl(this, _glowPlaySprite, _previewButtonBounds, Color.White);
            }
            else
            {
                if (_isPreviewing)
                    spriteBatch.DrawOnCtrl(this, _stopSprite, new Rectangle(SHEETBUTTON_WIDTH - 36, bounds.Height - BOTTOMSECTION_HEIGHT + 1, 32, 32), Color.White);
                else
                    spriteBatch.DrawOnCtrl(this, _playSprite, new Rectangle(SHEETBUTTON_WIDTH - 36, bounds.Height - BOTTOMSECTION_HEIGHT + 1, 32, 32), Color.White);
            }

            // Draw play button
            _practiceButtonBounds = new Rectangle(SHEETBUTTON_WIDTH - 73, bounds.Height - BOTTOMSECTION_HEIGHT + 1, 32, 32);
            if (_mouseOverPractice)
                spriteBatch.DrawOnCtrl(this, _glowBeatManiaSprite, _practiceButtonBounds, Color.White);
            else
                spriteBatch.DrawOnCtrl(this, _beatManiaSprite, _practiceButtonBounds, Color.White);

            // Draw emulate button
            _emulateButtonBounds = new Rectangle(SHEETBUTTON_WIDTH - 109, bounds.Height - BOTTOMSECTION_HEIGHT + 1, 32, 32);
            if (_mouseOverEmulate)
                spriteBatch.DrawOnCtrl(this, _glowAutoplaySprite, _emulateButtonBounds, Color.White);
            else
                spriteBatch.DrawOnCtrl(this, _autoplaySprite, _emulateButtonBounds, Color.White);
            
            // Draw bottom section separator
            spriteBatch.DrawOnCtrl(this, _dividerSprite, new Rectangle(0, bounds.Height - 40, bounds.Width, 8), Color.White);

            // Draw instrument icon
            if (Icon != null) {
                spriteBatch.DrawOnCtrl(this, this.Icon, new Rectangle((bounds.Height - BOTTOMSECTION_HEIGHT) / 2 - 32, (bounds.Height - 35) / 2 - 32, 64, 64), Color.White);
                // Draw icon box
                spriteBatch.DrawOnCtrl(this, _iconBoxSprite, new Rectangle(0, 0, iconSize, iconSize), Color.White);
            }

            // Wrap text
            string track = Title + @" - " + Artist;
            string wrappedText = DrawUtil.WrapText(Content.DefaultFont14, track, SHEETBUTTON_WIDTH - 40 - iconSize - 20);
            spriteBatch.DrawStringOnCtrl(this, wrappedText, Content.DefaultFont14, new Rectangle(89, 0, 216, this.Height - BOTTOMSECTION_HEIGHT), Color.White, false, true, 2, HorizontalAlignment.Left, VerticalAlignment.Middle);

            // Draw the user;
            spriteBatch.DrawStringOnCtrl(this, this.User, Content.DefaultFont14, new Rectangle(5, bounds.Height - BOTTOMSECTION_HEIGHT, USER_WIDTH, 35), Color.White, false, false, 0, HorizontalAlignment.Left, VerticalAlignment.Middle);
        }
    }
}