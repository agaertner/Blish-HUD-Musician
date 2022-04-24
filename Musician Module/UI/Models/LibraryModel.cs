using System;

namespace Nekres.Musician.UI.Models
{
    internal class LibraryModel : IDisposable
    {
        public readonly string DD_TITLE = "Title";
        public readonly string DD_ARTIST = "Artist";
        public readonly string DD_USER = "User";

        public readonly MusicSheetFactory MusicSheetFactory;
        public LibraryModel(MusicSheetFactory sheetFactory)
        {
            MusicSheetFactory = sheetFactory;
        }

        public void Dispose()
        {
        }
    }
}
