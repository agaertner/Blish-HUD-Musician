using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Nekres.Musician.UI.Models;
using Nekres.Musician.UI.Views;

namespace Nekres.Musician.UI.Presenters
{
    internal class LibraryPresenter : Presenter<LibraryView, LibraryModel>
    {
        public LibraryPresenter(LibraryView view, LibraryModel model) : base(view, model)
        {
            view.OnSelectedSortChanged += View_SelectedSortChanged;
        }

        private void View_SelectedSortChanged(object o, ValueEventArgs<string> e)
        {
            //TODO: Do sort
        }
    }
}
