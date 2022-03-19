using AnimeLibraryBrowser.Configuration;
using AnimeLibraryBrowser.Services;

using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnimeLibraryBrowser.Tests.Services
{
    [TestClass]
    public class FtpLinkFormatterTests
    {
        [DataTestMethod]
        [DataRow("ftp://server.domain")]
        [DataRow("ftp://server.domain/")]
        public void GetFtpLink_SimplePath_ReturnsFtpLink(string rootDirectoryFtpLink)
        {
            FtpLinkFormatter ftpLinkFormatter = CreateFtpLinkFormatter(rootDirectoryFtpLink);

            string expectedFtpLink = "ftp://server.domain/relative/path.extension";
            string actualFtpLink = ftpLinkFormatter.GetFtpLink("relative/path.extension");

            Assert.AreEqual(expectedFtpLink, actualFtpLink);
        }

        [DataTestMethod]
        [DataRow("ftp://server.domain")]
        [DataRow("ftp://server.domain/")]
        public void GetFtpLink_PathWithSpecialSymbols_ReturnsEscapedFtpLink(string rootDirectoryFtpLink)
        {
            FtpLinkFormatter ftpLinkFormatter = CreateFtpLinkFormatter(rootDirectoryFtpLink);

            string expectedFtpLink = "ftp://server.domain/relative%20path/with%20%5Bspecial%5D%20%22symbols%22%21";
            string actualFtpLink = ftpLinkFormatter.GetFtpLink("relative path/with [special] \"symbols\"!");

            Assert.AreEqual(expectedFtpLink, actualFtpLink);
        }

        private FtpLinkFormatter CreateFtpLinkFormatter(string rootDirectoryFtpLink)
        {
            IOptions<FtpLinkFormatterConfiguration> configurationOptions = new FtpLinkFormatterConfigurationOptionsMock(rootDirectoryFtpLink);

            return new FtpLinkFormatter(configurationOptions);
        }

        private class FtpLinkFormatterConfigurationOptionsMock : IOptions<FtpLinkFormatterConfiguration>
        {
            public FtpLinkFormatterConfiguration Value { get; }

            public FtpLinkFormatterConfigurationOptionsMock(string rootDirectoryFtpLink)
            {
                Value = new FtpLinkFormatterConfiguration() { RootDirectoryFtpLink = rootDirectoryFtpLink };
            }
        }
    }
}
