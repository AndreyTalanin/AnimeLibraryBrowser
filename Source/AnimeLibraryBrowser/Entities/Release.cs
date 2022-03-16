using System.Collections.Generic;

namespace AnimeLibraryBrowser.Entities
{
    public class Release
    {
        public string Title { get; set; }

        public int Year { get; set; }

        public string Type { get; set; }

        public int FrameWidth { get; set; }

        public int FrameHeight { get; set; }

        public string VideoEncoder { get; set; }

        public string AudioEncoder { get; set; }

        public List<FileGroup> FileGroups { get; set; } = new List<FileGroup>();
    }
}
