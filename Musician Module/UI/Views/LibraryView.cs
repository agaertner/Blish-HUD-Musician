using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Nekres.Musician.Core.Models;
using Nekres.Musician.UI.Presenters;
using System;
using Blish_HUD.Input;
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

        protected override async void Build(Container buildPanel)
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
                Size = new Point(buildPanel.Width, buildPanel.Height - 100),
                FlowDirection = ControlFlowDirection.LeftToRight,
                ControlPadding = new Vector2(5, 5),
                CanCollapse = false,
                CanScroll = true,
                Collapsed = false,
                ShowTint = true,
                ShowBorder = true,
            };

            foreach (var sheet in await MusicianModule.ModuleInstance.MusicSheetFactory.GetAll())
            {
                var sheetBtn = new SheetButton(sheet)
                {
                    Parent = MelodyFlowPanel
                };
                sheetBtn.OnPreviewClick += OnPreviewClick;
                sheetBtn.OnEmulateClick += OnEmulateClick;
            }

            var importBtn = new StandardButton
            {
                Parent = buildPanel,
                Size = new Point(100, 32),
                Location = new Point(MelodyFlowPanel.Right - 100, MelodyFlowPanel.Bottom + BOTTOM_MARGIN),
                Text = "Import XML"
            };
            importBtn.Click += OnImportBtnClick;
        }

        private void OnSortChanged(object o, ValueChangedEventArgs e) => OnSelectedSortChanged?.Invoke(o, new ValueEventArgs<string>(e.CurrentValue));

        private async void OnPreviewClick(object o, ValueEventArgs<bool> e)
        {
            var sheetBtn = (SheetButton)o;
            GameService.Overlay.BlishHudWindow.Hide();
            var sheet = await MusicianModule.ModuleInstance.MusicSheetFactory.GetById(sheetBtn.Id);
            await MusicianModule.ModuleInstance.MusicPlayer.PlayPreview(MusicSheet.FromModel(sheet));
        }
        private async void OnEmulateClick(object o, EventArgs e)
        {
            var sheetBtn = (SheetButton)o;
            GameService.Overlay.BlishHudWindow.Hide();
            var sheet = await MusicianModule.ModuleInstance.MusicSheetFactory.GetById(sheetBtn.Id);
            MusicianModule.ModuleInstance.MusicPlayer.PlayEmulate(MusicSheet.FromModel(sheet));
        }

        private async void OnImportBtnClick(object o, MouseEventArgs e)
        {
            var xml = await ClipboardUtil.WindowsClipboardService.GetTextAsync();
            if (!MusicSheet.TryParseXml(xml, out var sheet)) return;
            await MusicianModule.ModuleInstance.MusicSheetFactory.AddOrUpdate(sheet);
            var sheetBtn = new SheetButton(sheet.ToModel())
            {
                Parent = MelodyFlowPanel
            };
        }
    }
}
