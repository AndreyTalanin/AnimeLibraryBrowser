namespace AnimeLibraryBrowser.Entities
{
    public class File
    {
        public string Name { get; set; }

        public FileType Type { get; set; }

        public long Length { get; set; }

        public string FtpLink { get; set; }

        public string RelativePath { get; set; }
    }
}
