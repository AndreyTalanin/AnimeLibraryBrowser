using System;
using System.Text;
using System.Web;

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

            string urlEncodedPath = Uri.EscapeDataString(relativePath.Replace("\\", c_pathFragmentSeparator));
            stringBuilder.Append(urlEncodedPath);

            return stringBuilder.ToString();
        }
    }
}
