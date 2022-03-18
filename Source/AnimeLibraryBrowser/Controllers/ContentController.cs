using System.Threading.Tasks;

using AnimeLibraryBrowser.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AnimeLibraryBrowser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentDownloader m_contentDownloader;

        public ContentController(IContentDownloader contentDownloader)
        {
            m_contentDownloader = contentDownloader;
        }

        [HttpGet("file")]
        public async Task<FileResult> DownloadFileAsync([FromQuery] string relativePath)
        {
            m_contentDownloader.GetFileParameters(relativePath, out string fileName, out string contentType);
            return File(await m_contentDownloader.DownloadFileAsync(relativePath), contentType, fileName);
        }

        [HttpGet("directory")]
        public async Task<FileResult> DownloadDirectoryAsync([FromQuery] string relativePath)
        {
            m_contentDownloader.GetDirectoryParameters(relativePath, out string fileName, out string contentType);
            return File(await m_contentDownloader.DownloadDirectoryAsync(relativePath), contentType, fileName);
        }
    }
}
