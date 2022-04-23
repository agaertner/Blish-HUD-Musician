using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Nekres.Musician.Core.Models;
using Nekres.Musician.UI.Presenters;
using System;

namespace Nekres.Musician.UI.Views
{
    internal class LibraryView : View<LibraryPresenter>
    {
        internal event EventHandler<ValueEventArgs<string>> OnSelectedSortChanged;

        private const int TOP_MARGIN = 0;
        private const int RIGHT_MARGIN = 5;
        private const int BOTTOM_MARGIN = 10;
        private const int LEFT_MARGIN = 8;

        protected override void Build(Container buildPanel)
        {
            var melodyPanel = new FlowPanel
            {
                Location = new Point(0, BOTTOM_MARGIN),
                Size = new Point(buildPanel.Width, buildPanel.Size.Y - 50 - BOTTOM_MARGIN),
                Parent = buildPanel,
                ShowTint = true,
                ShowBorder = true,
                CanScroll = true
            };

            // Sort drop down
            var ddSortMethod = new Dropdown
            {
                Parent = buildPanel,
                Visible = melodyPanel.Visible,
                Location = new Point(buildPanel.Right - 150 - 10, 5),
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
        }

        private void OnSortChanged(object o, ValueChangedEventArgs e) => OnSelectedSortChanged?.Invoke(o, new ValueEventArgs<string>(e.CurrentValue));
    }
}
