using System;

using Microsoft.AspNetCore.Mvc;

namespace AnimeLibraryBrowser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [HttpGet]
        public FileResult DownloadFileAsync([FromQuery] string relativePath, [FromQuery] string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
