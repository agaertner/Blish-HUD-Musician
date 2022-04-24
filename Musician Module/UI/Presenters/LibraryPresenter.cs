using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Nekres.Musician.Controls;
using Nekres.Musician.Core.Models;
using Nekres.Musician.UI.Models;
using Nekres.Musician.UI.Views;

namespace Nekres.Musician.UI.Presenters
{
    internal class LibraryPresenter : Presenter<LibraryView, LibraryModel>
    {
        public LibraryPresenter(LibraryView view, LibraryModel model) : base(view, model)
        {
            view.OnSelectedSortChanged += View_SelectedSortChanged;
            model.MusicSheetFactory.OnSheetUpdated += OnSheetUpdated;
            model.MusicSheetFactory.OnSheetRemoved += OnSheetRemoved;
        }

        private void OnSheetUpdated(object o, ValueEventArgs<MusicSheetModel> e)
        {
            var sheet = this.View.MelodyFlowPanel.Children.Where(x => x.GetType() == typeof(SheetButton)).Cast<SheetButton>().FirstOrDefault(y => y.Id.Equals(e.Value.Id));
            if (sheet != null)
            {
                sheet.Artist = e.Value.Artist;
                sheet.Title = e.Value.Title;
                sheet.User = e.Value.User;
                return;
            }

            var sheetBtn = new SheetButton(e.Value)
            {
                Parent = this.View.MelodyFlowPanel
            };
        }
        
        private void OnSheetRemoved(object o, ValueEventArgs<Guid> e)
        {
            var sheet = this.View.MelodyFlowPanel.Children.Where(x => x.GetType() == typeof(SheetButton)).Cast<SheetButton>().FirstOrDefault(y => y.Id.Equals(e.Value));
            if (sheet == null) return;
            this.View.MelodyFlowPanel.RemoveChild(sheet);
        }

        private void View_SelectedSortChanged(object o, ValueEventArgs<string> e)
        {
            //TODO: Do sort
        }
    }
}
