namespace AnimeLibraryBrowser.Services.Interfaces
{
    public interface IReleaseDirectorySelector
    {
        public bool CompareReleaseTitle(string directoryName, string title);
    }
}
