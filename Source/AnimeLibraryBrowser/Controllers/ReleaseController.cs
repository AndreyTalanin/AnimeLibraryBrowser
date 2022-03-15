using System;
using System.Collections.Generic;

using AnimeLibraryBrowser.Entities;

using Microsoft.AspNetCore.Mvc;

namespace AnimeLibraryBrowser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReleaseController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Release>> GetAllReleasesAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{title}")]
        public string GetReleaseAsync(string title)
        {
            throw new NotImplementedException();
        }
    }
}
