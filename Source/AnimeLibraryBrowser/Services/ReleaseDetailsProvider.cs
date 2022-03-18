using System;
using System.Text.RegularExpressions;

using AnimeLibraryBrowser.Configuration;
using AnimeLibraryBrowser.Entities;
using AnimeLibraryBrowser.Services.Interfaces;

using Microsoft.Extensions.Options;

namespace AnimeLibraryBrowser.Services
{
    public class ReleaseDetailsProvider : IReleaseDetailsProvider
    {
        private readonly AnimeLibraryConfiguration m_configuration;

        public ReleaseDetailsProvider(IOptions<AnimeLibraryConfiguration> configurationOptions)
        {
            m_configuration = configurationOptions.Value;
        }

        public bool IsReleaseDirectory(string directoryName)
        {
            return Regex.IsMatch(directoryName, m_configuration.ReleaseDirectoryTemplate);
        }

        public Release ParseReleaseDetails(string directoryName)
        {
            Match match = Regex.Match(directoryName, m_configuration.ReleaseDirectoryTemplate);

            string title;
            if (match.Groups.TryGetValue(nameof(Release.Title), out Group titleGroup))
                title = titleGroup.Value;
            else
                throw new InvalidOperationException("Unable to find the required title capture group.");

            int year = match.Groups.TryGetValue(nameof(Release.Year), out Group yearGroup) ? int.Parse(yearGroup.Value) : 0;
            string type = match.Groups.TryGetValue(nameof(Release.Type), out Group typeGroup) ? typeGroup.Value : string.Empty;
            int frameWidth = match.Groups.TryGetValue(nameof(Release.FrameWidth), out Group frameWidthGroup) ? int.Parse(frameWidthGroup.Value) : 0;
            int frameHeight = match.Groups.TryGetValue(nameof(Release.FrameHeight), out Group frameHeightGroup) ? int.Parse(frameHeightGroup.Value) : 0;
            string videoEncoder = match.Groups.TryGetValue(nameof(Release.VideoEncoder), out Group videoEncoderGroup) ? videoEncoderGroup.Value : string.Empty;
            string audioEncoder = match.Groups.TryGetValue(nameof(Release.AudioEncoder), out Group audioEncoderGroup) ? audioEncoderGroup.Value : string.Empty;

            return new Release()
            {
                Title = title,
                Year = year,
                Type = type,
                FrameWidth = frameWidth,
                FrameHeight = frameHeight,
                VideoEncoder = videoEncoder,
                AudioEncoder = audioEncoder,
            };
        }
    }
}
