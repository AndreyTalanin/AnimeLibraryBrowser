using AnimeLibraryBrowser.Configuration;
using AnimeLibraryBrowser.Services;

using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnimeLibraryBrowser.Tests.Services
{
    [TestClass]
    public class ReleaseDirectorySelectorTests
    {
        private const string c_releaseDirectorySearchTemplate = "{0}\\s\\(\\d{{4}}\\)\\s\\[.+\\]";

        [DataTestMethod]
        [DataRow("Angel Beats! (2010) [BDRip, 1920x1080, AVC, FLAC]", "Angel Beats!", true)]
        [DataRow("Angel Beats! (2010) [BDRip, 1920x1080, AVC, FLAC]", "Angel Beats.", false)]
        [DataRow("K-On! (2009) [BDRip, 1920x1080, AVC, FLAC]", "K-On!", true)]
        [DataRow("K-On! (2009) [BDRip, 1920x1080, AVC, FLAC]", "K-On!!", false)]
        [DataRow("KonoSuba - God's Blessing on This Wonderful World! (2016) [BDRip, 1920x1080, HEVC, FLAC]", "KonoSuba - God's Blessing on This Wonderful World!", true)]
        [DataRow("KonoSuba - God's Blessing on This Wonderful World! (2016) [BDRip, 1920x1080, HEVC, FLAC]", "KonoSuba - God's Blessing on This Wonderful World! (2016)", false)]
        public void CompareReleaseTitle_DefaultSearchPattern_ReturnsTrueIfMatched(string directoryName, string title, bool matchesReleaseTitle)
        {
            ReleaseDirectorySelector releaseDirectorySelector = CreateReleaseDirectorySelector();

            bool actualResult = releaseDirectorySelector.CompareReleaseTitle(directoryName, title);

            Assert.AreEqual(matchesReleaseTitle, actualResult);
        }

        private ReleaseDirectorySelector CreateReleaseDirectorySelector()
        {
            IOptions<AnimeLibraryConfiguration> configurationOptions = new AnimeLibraryConfigurationOptionsMock();

            return new ReleaseDirectorySelector(configurationOptions);
        }

        private class AnimeLibraryConfigurationOptionsMock : IOptions<AnimeLibraryConfiguration>
        {
            public AnimeLibraryConfiguration Value { get; }

            public AnimeLibraryConfigurationOptionsMock()
            {
                Value = new AnimeLibraryConfiguration() { ReleaseDirectorySearchTemplate = c_releaseDirectorySearchTemplate };
            }
        }
    }
}
