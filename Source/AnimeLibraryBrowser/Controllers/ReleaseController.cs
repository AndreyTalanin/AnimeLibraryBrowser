using System.Collections.Generic;
using System.Threading.Tasks;

using AnimeLibraryBrowser.Entities;
using AnimeLibraryBrowser.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AnimeLibraryBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReleaseController : ControllerBase
    {
        private readonly IDirectoryTreeAnalyzer m_directoryTreeAnalyzer;

        public ReleaseController(IDirectoryTreeAnalyzer directoryTreeAnalyzer)
        {
            m_directoryTreeAnalyzer = directoryTreeAnalyzer;
        }

        [HttpGet]
        public async Task<ActionResult<List<Release>>> GetAllReleasesAsync()
        {
            List<Release> releases = await m_directoryTreeAnalyzer.GetAllReleasesAsync();
            return Ok(releases);
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<Release>> GetReleaseAsync(string title)
        {
            Release release = await m_directoryTreeAnalyzer.GetReleaseAsync(title);
            return Ok(release);
        }
    }
}
