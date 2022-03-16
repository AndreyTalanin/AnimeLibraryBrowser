using AnimeLibraryBrowser.Entities;

namespace AnimeLibraryBrowser.Services.Interfaces
{
    public interface IReleaseDetailsProvider
    {
        public bool IsReleaseDirectory(string directoryName);

        public Release ParseReleaseDetails(string directoryName);
    }
}
