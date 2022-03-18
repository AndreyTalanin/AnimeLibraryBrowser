using System.Text.RegularExpressions;

using AnimeLibraryBrowser.Configuration;
using AnimeLibraryBrowser.Services.Interfaces;

using Microsoft.Extensions.Options;

namespace AnimeLibraryBrowser.Services
{
    public class ReleaseDirectorySelector : IReleaseDirectorySelector
    {
        private readonly AnimeLibraryConfiguration m_configuration;

        public ReleaseDirectorySelector(IOptions<AnimeLibraryConfiguration> configurationOptions)
        {
            m_configuration = configurationOptions.Value;
        }

        public bool CompareReleaseTitle(string directoryName, string title)
        {
            string pattern = string.Format(m_configuration.ReleaseDirectorySearchTemplate, Regex.Escape(title));
            return Regex.IsMatch(directoryName, pattern);
        }
    }
}
