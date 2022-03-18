using System.Collections.Generic;
using System.Threading.Tasks;

using AnimeLibraryBrowser.Entities;

namespace AnimeLibraryBrowser.Services.Interfaces
{
    public interface IDirectoryTreeAnalyzer
    {
        public Task<List<Release>> GetAllReleasesAsync();

        public Task<Release> GetReleaseAsync(string title);
    }
}
