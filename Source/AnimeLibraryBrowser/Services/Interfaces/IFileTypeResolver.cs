using AnimeLibraryBrowser.Entities;

namespace AnimeLibraryBrowser.Services.Interfaces
{
    public interface IFileTypeResolver
    {
        public FileType ResolveFileType(string fileName);
    }
}
