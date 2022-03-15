using System.Collections.Generic;

namespace AnimeLibraryBrowser.Entities
{
    public class FileGroup
    {
        public string Name { get; set; }

        public List<File> Files { get; set; } = new List<File>();
    }
}
