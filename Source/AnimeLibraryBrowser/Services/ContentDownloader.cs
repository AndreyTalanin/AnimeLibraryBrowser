﻿using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

using AnimeLibraryBrowser.Configuration;
using AnimeLibraryBrowser.Services.Interfaces;

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace AnimeLibraryBrowser.Services
{
    public class ContentDownloader : IContentDownloader
    {
        private const int c_maxArchiveDepth = 20;
        private const string c_defaultContentType = "application/octet-stream";
        private const string c_archiveContentType = "application/zip";

        private readonly AnimeLibraryConfiguration m_configuration;
        private readonly IContentTypeProvider m_contentTypeProvider;

        public ContentDownloader(IOptions<AnimeLibraryConfiguration> configurationOptions, IContentTypeProvider contentTypeProvider)
        {
            m_configuration = configurationOptions.Value;
            m_contentTypeProvider = contentTypeProvider;
        }

        public async Task<Stream> DownloadFileAsync(string relativePath)
        {
            string absolutePath = GetAbsolutePath(relativePath);
            return await CreateMemoryStreamAsync(async (memoryStream) =>
            {
                using FileStream fileStream = new FileStream(absolutePath, FileMode.Open, FileAccess.Read);
                await fileStream.CopyToAsync(memoryStream);
            });
        }

        public void GetFileParameters(string relativePath, out string fileName, out string contentType)
        {
            FileInfo file = new FileInfo(Path.Combine(m_configuration.RootDirectory, relativePath));

            fileName = file.Name;
            if (!m_contentTypeProvider.TryGetContentType(file.Name, out contentType))
                contentType = c_defaultContentType;
        }

        public async Task<Stream> DownloadDirectoryAsync(string relativePath)
        {
            string absolutePath = GetAbsolutePath(relativePath);
            return await CreateMemoryStreamAsync(async (memoryStream) =>
            {
                await WriteDirectoryArchiveToStreamAsync(memoryStream, absolutePath);
            });
        }

        public void GetDirectoryParameters(string relativePath, out string fileName, out string contentType)
        {
            DirectoryInfo directory = new DirectoryInfo(Path.Combine(m_configuration.RootDirectory, relativePath));

            fileName = $"{directory.Name}.zip";
            if (!m_contentTypeProvider.TryGetContentType(fileName, out contentType))
                contentType = c_archiveContentType;
        }

        private string GetAbsolutePath(string relativePath)
        {
            string absolutePath = Path.GetFullPath(Path.Combine(m_configuration.RootDirectory, relativePath));
            if (!absolutePath.StartsWith(m_configuration.RootDirectory, StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException("Unable to download files outside of the root directory.");
            else
                return absolutePath;
        }

        private async Task<MemoryStream> CreateMemoryStreamAsync(MemoryStreamAsyncAction asyncAction)
        {
            MemoryStream memoryStream = new MemoryStream();

            await asyncAction(memoryStream);

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        private async Task WriteDirectoryArchiveToStreamAsync(Stream stream, string rootDirectoryPath)
        {
            DirectoryInfo rootDirectory = new DirectoryInfo(rootDirectoryPath);
            using ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create, true);

            async Task ProcessDirectoryAsync(int level, DirectoryInfo directory)
            {
                string relativePath = Path.GetRelativePath(rootDirectory.FullName, directory.FullName);
                string relativeDirectoryPath = level == 0 ? rootDirectory.Name : Path.Combine(rootDirectory.Name, relativePath);

                foreach (FileInfo file in directory.GetFiles())
                {
                    string relativeFilePath = Path.Combine(relativeDirectoryPath, file.Name);
                    using Stream entryStream = archive.CreateEntry(relativeFilePath, CompressionLevel.Fastest).Open();
                    using FileStream fileStream = file.OpenRead();
                    await fileStream.CopyToAsync(entryStream);
                }

                if (level < c_maxArchiveDepth)
                {
                    foreach (DirectoryInfo childDirectory in directory.GetDirectories())
                        await ProcessDirectoryAsync(level + 1, childDirectory);
                }
            }

            await ProcessDirectoryAsync(0, rootDirectory);
        }

        private delegate Task MemoryStreamAsyncAction(MemoryStream memoryStream);
    }
}
