using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace AnimeLibraryBrowser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        [HttpGet("file")]
        public Task<FileResult> DownloadFileAsync([FromQuery] string relativePath)
        {
            throw new NotImplementedException();
        }

        [HttpGet("directory")]
        public Task<FileResult> DownloadDirectoryAsync([FromQuery] string relativePath)
        {
            throw new NotImplementedException();
        }
    }
}
