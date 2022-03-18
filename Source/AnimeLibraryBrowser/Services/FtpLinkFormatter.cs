using System;
using System.IO;
using System.Linq;
using System.Text;

using AnimeLibraryBrowser.Configuration;
using AnimeLibraryBrowser.Services.Interfaces;

using Microsoft.Extensions.Options;

namespace AnimeLibraryBrowser.Services
{
    public class FtpLinkFormatter : IFtpLinkFormatter
    {
        private const string c_pathFragmentSeparator = "/";

        private readonly FtpLinkFormatterConfiguration m_configuration;

        public FtpLinkFormatter(IOptions<FtpLinkFormatterConfiguration> configurationOptions)
        {
            m_configuration = configurationOptions.Value;
        }

        public string GetFtpLink(string relativePath)
        {
            StringBuilder stringBuilder = new StringBuilder(m_configuration.RootDirectoryFtpLink);

            if (!m_configuration.RootDirectoryFtpLink.EndsWith(c_pathFragmentSeparator))
                stringBuilder.Append(c_pathFragmentSeparator);

            string[] pathFragments = relativePath
                .Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar })
                .Select(pathFragment => Uri.EscapeDataString(pathFragment))
                .ToArray();

            string urlEncodedPath = string.Join(c_pathFragmentSeparator, pathFragments);
            stringBuilder.Append(urlEncodedPath);

            return stringBuilder.ToString();
        }
    }
}
