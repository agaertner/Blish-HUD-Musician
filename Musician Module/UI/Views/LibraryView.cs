using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Nekres.Musician.Core.Models;
using Nekres.Musician.UI.Presenters;
using System;
using Nekres.Musician.Controls;
using Nekres.Musician.UI.Models;

namespace Nekres.Musician.UI.Views
{
    internal class LibraryView : View<LibraryPresenter>
    {
        internal event EventHandler<ValueEventArgs<string>> OnSelectedSortChanged;

        private const int TOP_MARGIN = 0;
        private const int RIGHT_MARGIN = 5;
        private const int BOTTOM_MARGIN = 10;
        private const int LEFT_MARGIN = 8;

        public FlowPanel MelodyFlowPanel { get; private set; }

        public LibraryView(LibraryModel model)
        {
            this.WithPresenter(new LibraryPresenter(this, model));
        }

        protected override void Build(Container buildPanel)
        {
            // Sort drop down
            var ddSortMethod = new Dropdown
            {
                Parent = buildPanel,
                Location = new Point(buildPanel.Right - RIGHT_MARGIN - 150, TOP_MARGIN),
                Width = 150
            };
            ddSortMethod.Items.Add(this.Presenter.Model.DD_TITLE);
            ddSortMethod.Items.Add(this.Presenter.Model.DD_ARTIST);
            ddSortMethod.Items.Add(this.Presenter.Model.DD_USER);
            ddSortMethod.Items.Add("------------------");
            foreach (var instrument in Enum.GetNames(typeof(Instrument)))
                ddSortMethod.Items.Add(instrument);
            ddSortMethod.ValueChanged += OnSortChanged;
            ddSortMethod.SelectedItem = this.Presenter.Model.DD_TITLE;
            OnSortChanged(ddSortMethod, new ValueChangedEventArgs(string.Empty, ddSortMethod.SelectedItem));

            MelodyFlowPanel = new FlowPanel
            {
                Parent = buildPanel,
                Location = new Point(0, ddSortMethod.Bottom + BOTTOM_MARGIN),
                Size = new Point(buildPanel.Width, buildPanel.Height),
                FlowDirection = ControlFlowDirection.LeftToRight,
                ControlPadding = new Vector2(5, 5),
                CanCollapse = false,
                CanScroll = true,
                Collapsed = false,
                ShowTint = true,
                ShowBorder = true,
            };

            foreach (var (id, sheet) in MusicianModule.ModuleInstance.MusicSheetFactory.Index)
            {
                var sheetBtn = new SheetButton(sheet)
                {
                    Parent = MelodyFlowPanel
                };
                sheetBtn.OnPreviewClick += OnPreviewClick;
                sheetBtn.OnEmulateClick += OnEmulateClick;
            }
        }

        private void OnSortChanged(object o, ValueChangedEventArgs e) => OnSelectedSortChanged?.Invoke(o, new ValueEventArgs<string>(e.CurrentValue));

        private async void OnPreviewClick(object o, ValueEventArgs<bool> e)
        {
            var sheetBtn = (SheetButton)o;
            GameService.Overlay.BlishHudWindow.Hide();
            var sheet = await MusicianModule.ModuleInstance.MusicSheetFactory.FromCache(sheetBtn.Id);
            MusicianModule.ModuleInstance.MusicPlayer.PlayPreview(sheet);
        }
        private async void OnEmulateClick(object o, EventArgs e)
        {
            var sheetBtn = (SheetButton)o;
            GameService.Overlay.BlishHudWindow.Hide();
            var sheet = await MusicianModule.ModuleInstance.MusicSheetFactory.FromCache(sheetBtn.Id);
            MusicianModule.ModuleInstance.MusicPlayer.PlayEmulate(sheet);
        }
    }
}
