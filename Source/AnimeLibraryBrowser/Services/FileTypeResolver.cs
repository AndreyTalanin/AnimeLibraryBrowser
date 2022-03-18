using System;
using System.Collections.Generic;
using System.IO;

using AnimeLibraryBrowser.Configuration;
using AnimeLibraryBrowser.Entities;
using AnimeLibraryBrowser.Services.Interfaces;

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AnimeLibraryBrowser.Services
{
    public class FileTypeResolver : IFileTypeResolver
    {
        private readonly AnimeLibraryConfiguration m_configuration;
        private readonly IContentTypeProvider m_contentTypeProvider;
        private readonly IMemoryCache m_memoryCache;

        public FileTypeResolver(IOptions<AnimeLibraryConfiguration> configurationOptions, IContentTypeProvider contentTypeProvider, IMemoryCache memoryCache)
        {
            m_configuration = configurationOptions.Value;
            m_contentTypeProvider = contentTypeProvider;
            m_memoryCache = memoryCache;
        }

        public FileType ResolveFileType(string fileName)
        {
            string extension = Path.GetExtension(fileName);

            HashSet<string> videoFileExtensions = m_memoryCache.GetOrCreate(nameof(AnimeLibraryConfiguration.VideoFileExtensions), cacheEntry =>
            {
                HashSet<string> set = new HashSet<string>(m_configuration.VideoFileExtensions, StringComparer.InvariantCultureIgnoreCase);
                cacheEntry.SetValue(set);
                return set;
            });
            HashSet<string> audioFileExtensions = m_memoryCache.GetOrCreate(nameof(AnimeLibraryConfiguration.AudioFileExtensions), cacheEntry =>
            {
                HashSet<string> set = new HashSet<string>(m_configuration.AudioFileExtensions, StringComparer.InvariantCultureIgnoreCase);
                cacheEntry.SetValue(set);
                return set;
            });
            HashSet<string> subtitlesFileExtensions = m_memoryCache.GetOrCreate(nameof(AnimeLibraryConfiguration.SubtitlesFileExtensions), cacheEntry =>
            {
                HashSet<string> set = new HashSet<string>(m_configuration.SubtitlesFileExtensions, StringComparer.InvariantCultureIgnoreCase);
                cacheEntry.SetValue(set);
                return set;
            });

            if (videoFileExtensions.Contains(extension))
                return FileType.Video;
            if (audioFileExtensions.Contains(extension))
                return FileType.Audio;
            if (subtitlesFileExtensions.Contains(extension))
                return FileType.Subtitles;

            if (m_contentTypeProvider.TryGetContentType(fileName, out string contentType))
            {
                if (contentType.Contains("video/"))
                    return FileType.Video;
                if (contentType.Contains("audio/"))
                    return FileType.Audio;
            }

            return FileType.Other;
        }
    }
}
