using System.IO;
using System.Threading.Tasks;

namespace AnimeLibraryBrowser.Services.Interfaces
{
    public interface IContentDownloader
    {
        public Task<Stream> DownloadFileAsync(string relativePath);

        public void GetFileParameters(string relativePath, out string fileName, out string contentType);

        public Task<Stream> DownloadDirectoryAsync(string relativePath);

        public void GetDirectoryParameters(string relativePath, out string fileName, out string contentType);
    }
}
