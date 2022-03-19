using System;
using System.Collections.Generic;
using System.IO;

using AnimeLibraryBrowser.Configuration;
using AnimeLibraryBrowser.Entities;
using AnimeLibraryBrowser.Services;

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnimeLibraryBrowser.Tests.Services
{
    [TestClass]
    public class FileTypeResolverTests
    {
        [DataTestMethod]
        [DataRow(".avi", FileType.Video)]
        [DataRow(".flac", FileType.Audio)]
        [DataRow(".srt", FileType.Subtitles)]
        public void ResolveFileType_ConfiguredExtensions_ReturnsMappedFileType(string extension, FileType fileType)
        {
            FileTypeResolver fileTypeResolver = CreateFileTypeResolver();

            FileType actualFileType = fileTypeResolver.ResolveFileType("FileName" + extension);

            Assert.AreEqual(fileType, actualFileType);
        }

        [DataTestMethod]
        [DataRow(".mkv", FileType.Video)]
        [DataRow(".mka", FileType.Audio)]
        public void ResolveFileType_ExternalResolverKnownExtensions_ReturnsMappedFileType(string extension, FileType fileType)
        {
            FileTypeResolver fileTypeResolver = CreateFileTypeResolver();

            FileType actualFileType = fileTypeResolver.ResolveFileType("FileName" + extension);

            Assert.AreEqual(fileType, actualFileType);
        }

        [DataTestMethod]
        [DataRow(".txt")]
        [DataRow(".bin")]
        public void ResolveFileType_UnknownExtensions_ReturnsOtherFileType(string extension)
        {
            FileTypeResolver fileTypeResolver = CreateFileTypeResolver();

            FileType expectedFileType = FileType.Other;
            FileType actualFileType = fileTypeResolver.ResolveFileType("FileName" + extension);

            Assert.AreEqual(expectedFileType, actualFileType);
        }

        private FileTypeResolver CreateFileTypeResolver()
        {
            IOptions<AnimeLibraryConfiguration> configurationOptions = new AnimeLibraryConfigurationOptionsMock();
            IContentTypeProvider contentTypeProvider = new ContentTypeProviderMock();
            IMemoryCache memoryCache = new MemoryCacheMock();

            return new FileTypeResolver(configurationOptions, contentTypeProvider, memoryCache);
        }

        private class AnimeLibraryConfigurationOptionsMock : IOptions<AnimeLibraryConfiguration>
        {
            public AnimeLibraryConfiguration Value { get; }

            public AnimeLibraryConfigurationOptionsMock()
            {
                Value = new AnimeLibraryConfiguration()
                {
                    VideoFileExtensions = new List<string>() { ".avi" },
                    AudioFileExtensions = new List<string>() { ".flac" },
                    SubtitlesFileExtensions = new List<string>() { ".srt", },
                };
            }
        }

        private class ContentTypeProviderMock : IContentTypeProvider
        {
            public bool TryGetContentType(string subpath, out string contentType)
            {
                bool result;
                string extension = Path.GetExtension(subpath).ToLower();
                (result, contentType) = extension switch
                {
                    ".mkv" => (true, "video/x-matroska"),
                    ".mka" => (true, "audio/x-matroska"),
                    _ => (false, null),
                };
                return result;
            }
        }

        private class MemoryCacheMock : IMemoryCache
        {
            private readonly Dictionary<object, ICacheEntry> m_cache = new Dictionary<object, ICacheEntry>();

            public ICacheEntry CreateEntry(object key)
            {
                CacheEntryMock cacheEntry = new CacheEntryMock(key);
                m_cache.Add(key, cacheEntry);
                return cacheEntry;
            }
            public void Remove(object key)
            {
                m_cache.Remove(key);
            }
            public bool TryGetValue(object key, out object value)
            {
                bool result;
                (result, value) = m_cache.TryGetValue(key, out ICacheEntry cacheEntry) ? (true, cacheEntry.Value) : (false, null);
                return result;
            }
            public void Dispose()
            {
            }

            private class CacheEntryMock : ICacheEntry
            {
                public object Key { get; }
                public object Value { get; set; }
                public long? Size { get; set; }
                public IList<IChangeToken> ExpirationTokens { get; }
                public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; }

                public DateTimeOffset? AbsoluteExpiration { get; set; }
                public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
                public TimeSpan? SlidingExpiration { get; set; }
                public CacheItemPriority Priority { get; set; }

                public CacheEntryMock(object key)
                {
                    Key = key;
                }

                public void Dispose()
                {
                }
            }
        }
    }
}
