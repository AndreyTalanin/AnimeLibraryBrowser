using AnimeLibraryBrowser.Configuration;
using AnimeLibraryBrowser.Entities;
using AnimeLibraryBrowser.Services;

using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnimeLibraryBrowser.Tests.Services
{
    [TestClass]
    public class ReleaseDetailsProviderTests
    {
        private const string c_requiredRegularExpression = "(?'Title'.+)";
        private const string c_fullRegularExpression = "(?'Title'.+)\\s\\((?'Year'\\d{4})\\)\\s\\[(?'Type'.+),\\s(?'FrameWidth'\\d{1,5})x(?'FrameHeight'\\d{1,5}),\\s(?'VideoEncoder'.+),\\s(?'AudioEncoder'.+)\\]";

        [DataTestMethod]
        [DataRow("Angel Beats! (2010) [BDRip, 1920x1080, AVC, FLAC]")]
        [DataRow("2010 - Angel Beats! [BDRip, 1920x1080, AVC, FLAC]")]
        [DataRow("I Want to Eat Your Pancreas (2018) [BDRip, 1920x1080, HEVC, FLAC]")]
        [DataRow("I Want to Eat Your Pancreas - 2018 - (BDRip, 1920x1080, HEVC, FLAC)")]
        public void IsReleaseDirectory_RequiredPattern_ReturnsTrue(string directoryName)
        {
            ReleaseDetailsProvider releaseDetailsProvider = CreateReleaseDetailsProvider(c_requiredRegularExpression);

            bool expectedResult = true;
            bool actualResult = releaseDetailsProvider.IsReleaseDirectory(directoryName);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [DataTestMethod]
        [DataRow("Angel Beats! (2010) [BDRip, 1920x1080, AVC, FLAC]", true)]
        [DataRow("2010 - Angel Beats! [BDRip, 1920x1080, AVC, FLAC]", false)]
        [DataRow("I Want to Eat Your Pancreas (2018) [BDRip, 1920x1080, HEVC, FLAC]", true)]
        [DataRow("I Want to Eat Your Pancreas - 2018 - (BDRip, 1920x1080, HEVC, FLAC)", false)]
        public void IsReleaseDirectory_FullPattern_ReturnsTrueIfMatched(string directoryName, bool matchesRegularExpression)
        {
            ReleaseDetailsProvider releaseDetailsProvider = CreateReleaseDetailsProvider(c_fullRegularExpression);

            bool actualResult = releaseDetailsProvider.IsReleaseDirectory(directoryName);

            Assert.AreEqual(matchesRegularExpression, actualResult);
        }

        [DataTestMethod]
        [DataRow("Angel Beats! (2010) [BDRip, 1920x1080, AVC, FLAC]")]
        [DataRow("I Want to Eat Your Pancreas (2018) [BDRip, 1920x1080, HEVC, FLAC]")]
        [DataRow("Violet Evergarden (2018) [BDRip, 1920x1080, AVC, FLAC]")]
        [DataRow("Your Lie in April (2015) [BDRip, 1920x1080, AVC, FLAC]")]
        [DataRow("Your Name (2017) [BDRip, 1920x1080, AVC, FLAC]")]
        public void ParseReleaseDetails_RequiredPattern_ReturnsTitleOnly(string directoryName)
        {
            ReleaseDetailsProvider releaseDetailsProvider = CreateReleaseDetailsProvider(c_requiredRegularExpression);

            Release expectedRelease = new Release()
            {
                Title = directoryName,
                Year = 0,
                Type = string.Empty,
                FrameWidth = 0,
                FrameHeight = 0,
                VideoEncoder = string.Empty,
                AudioEncoder = string.Empty,
            };
            Release actualRelease = releaseDetailsProvider.ParseReleaseDetails(directoryName);

            Assert.AreEqual(expectedRelease.Title, actualRelease.Title);
            Assert.AreEqual(expectedRelease.Year, actualRelease.Year);
            Assert.AreEqual(expectedRelease.Type, actualRelease.Type);
            Assert.AreEqual(expectedRelease.FrameWidth, actualRelease.FrameWidth);
            Assert.AreEqual(expectedRelease.FrameHeight, actualRelease.FrameHeight);
            Assert.AreEqual(expectedRelease.VideoEncoder, actualRelease.VideoEncoder);
            Assert.AreEqual(expectedRelease.AudioEncoder, actualRelease.AudioEncoder);
        }

        [DataTestMethod]
        [DataRow("Angel Beats! (2010) [BDRip, 1920x1080, AVC, FLAC]", "Angel Beats!", 2010, "BDRip", 1920, 1080, "AVC", "FLAC")]
        [DataRow("I Want to Eat Your Pancreas (2018) [BDRip, 1920x1080, HEVC, FLAC]", "I Want to Eat Your Pancreas", 2018, "BDRip", 1920, 1080, "HEVC", "FLAC")]
        [DataRow("Violet Evergarden (2018) [BDRip, 1920x1080, AVC, FLAC]", "Violet Evergarden", 2018, "BDRip", 1920, 1080, "AVC", "FLAC")]
        [DataRow("Your Lie in April (2015) [BDRip, 1920x1080, AVC, FLAC]", "Your Lie in April", 2015, "BDRip", 1920, 1080, "AVC", "FLAC")]
        [DataRow("Your Name (2017) [BDRip, 1920x1080, AVC, FLAC]", "Your Name", 2017, "BDRip", 1920, 1080, "AVC", "FLAC")]
        public void ParseReleaseDetails_FullPattern_ReturnsFullReleaseDetails(string directoryName, string title, int year, string type, int frameWidth, int frameHeight, string videoEncoder, string audioEncoder)
        {
            ReleaseDetailsProvider releaseDetailsProvider = CreateReleaseDetailsProvider(c_fullRegularExpression);

            Release expectedRelease = new Release()
            {
                Title = title,
                Year = year,
                Type = type,
                FrameWidth = frameWidth,
                FrameHeight = frameHeight,
                VideoEncoder = videoEncoder,
                AudioEncoder = audioEncoder,
            };
            Release actualRelease = releaseDetailsProvider.ParseReleaseDetails(directoryName);

            Assert.AreEqual(expectedRelease.Title, actualRelease.Title);
            Assert.AreEqual(expectedRelease.Year, actualRelease.Year);
            Assert.AreEqual(expectedRelease.Type, actualRelease.Type);
            Assert.AreEqual(expectedRelease.FrameWidth, actualRelease.FrameWidth);
            Assert.AreEqual(expectedRelease.FrameHeight, actualRelease.FrameHeight);
            Assert.AreEqual(expectedRelease.VideoEncoder, actualRelease.VideoEncoder);
            Assert.AreEqual(expectedRelease.AudioEncoder, actualRelease.AudioEncoder);
        }

        private ReleaseDetailsProvider CreateReleaseDetailsProvider(string releaseDirectoryTemplate)
        {
            IOptions<AnimeLibraryConfiguration> configurationOptions = new AnimeLibraryConfigurationOptionsMock(releaseDirectoryTemplate);

            return new ReleaseDetailsProvider(configurationOptions);
        }

        private class AnimeLibraryConfigurationOptionsMock : IOptions<AnimeLibraryConfiguration>
        {
            public AnimeLibraryConfiguration Value { get; }

            public AnimeLibraryConfigurationOptionsMock(string releaseDirectoryTemplate)
            {
                Value = new AnimeLibraryConfiguration() { ReleaseDirectoryTemplate = releaseDirectoryTemplate };
            }
        }
    }
}
