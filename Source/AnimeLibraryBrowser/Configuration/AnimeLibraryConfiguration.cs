using System.Collections.Generic;

namespace AnimeLibraryBrowser.Configuration
{
    public class AnimeLibraryConfiguration
    {
        public string RootDirectory { get; set; }

        public string ReleaseDirectoryTemplate { get; set; }
        public string ReleaseDirectorySearchTemplate { get; set; }

        public List<string> VideoFileExtensions { get; set; } = new List<string>();
        public List<string> AudioFileExtensions { get; set; } = new List<string>();
        public List<string> SubtitlesFileExtensions { get; set; } = new List<string>();
    }
}
