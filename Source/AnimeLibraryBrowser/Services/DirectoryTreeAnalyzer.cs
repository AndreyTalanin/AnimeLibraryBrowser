using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AnimeLibraryBrowser.Configuration;
using AnimeLibraryBrowser.Entities;
using AnimeLibraryBrowser.Services.Interfaces;

using Microsoft.Extensions.Options;

using File = AnimeLibraryBrowser.Entities.File;

namespace AnimeLibraryBrowser.Services
{
    public class DirectoryTreeAnalyzer : IDirectoryTreeAnalyzer
    {
        private const int c_maxSearchDepth = 20;
        private const string с_rootDirectoryFileGroupName = "<Root Directory>";
        private const string c_pathFragmentSeparator = " \\ ";

        private readonly AnimeLibraryConfiguration m_configuration;
        private readonly IReleaseDetailsProvider m_releaseDetailsProvider;
        private readonly IReleaseDirectorySelector m_releaseDirectorySelector;
        private readonly IFileTypeResolver m_fileTypeResolver;
        private readonly IFtpLinkFormatter m_ftpLinkFormatter;

        public DirectoryTreeAnalyzer(
            IOptions<AnimeLibraryConfiguration> configurationOptions,
            IReleaseDetailsProvider releaseDetailsProvider,
            IReleaseDirectorySelector releaseDirectorySelector,
            IFileTypeResolver fileTypeResolver,
            IFtpLinkFormatter ftpLinkFormatter
            )
        {
            m_configuration = configurationOptions.Value;
            m_releaseDetailsProvider = releaseDetailsProvider;
            m_releaseDirectorySelector = releaseDirectorySelector;
            m_fileTypeResolver = fileTypeResolver;
            m_ftpLinkFormatter = ftpLinkFormatter;
        }

        public async Task<List<Release>> GetAllReleasesAsync()
        {
            return await Task.Run(() =>
            {
                DirectoryInfo rootDirectory = new DirectoryInfo(m_configuration.RootDirectory);

                List<Release> releases = new List<Release>();
                foreach (DirectoryInfo directory in rootDirectory.GetDirectories())
                {
                    if (m_releaseDetailsProvider.IsReleaseDirectory(directory.Name))
                    {
                        Release release = m_releaseDetailsProvider.ParseReleaseDetails(directory.Name);
                        releases.Add(release);
                    }
                }
                return releases;
            });
        }

        public async Task<Release> GetReleaseAsync(string title)
        {
            return await Task.Run(() =>
            {
                DirectoryInfo rootDirectory = new DirectoryInfo(m_configuration.RootDirectory);
                DirectoryInfo releaseDirectory = rootDirectory.GetDirectories()
                    .Where(directory => m_releaseDirectorySelector.CompareReleaseTitle(directory.Name, title))
                    .Single();

                if (m_releaseDetailsProvider.IsReleaseDirectory(releaseDirectory.Name))
                {
                    Release release = m_releaseDetailsProvider.ParseReleaseDetails(releaseDirectory.Name);

                    List<string> pathLevels = new List<string>();
                    List<FileGroup> fileGroups = new List<FileGroup>();

                    void ProcessDirectoryTree(int level, DirectoryInfo directory)
                    {
                        if (level > 0)
                            pathLevels.Add(directory.Name);

                        string fileGroupName = level == 0 ? с_rootDirectoryFileGroupName : string.Join(c_pathFragmentSeparator, pathLevels);

                        FileGroup fileGroup = new FileGroup()
                        {
                            Name = fileGroupName,
                            RelativePath = Path.GetRelativePath(m_configuration.RootDirectory, directory.FullName),
                        };

                        foreach (FileInfo file in directory.GetFiles())
                        {
                            FileType fileType = m_fileTypeResolver.ResolveFileType(file.Name);

                            string relativePath = Path.GetRelativePath(m_configuration.RootDirectory, file.FullName);
                            string ftpLink = m_ftpLinkFormatter.GetFtpLink(relativePath);

                            fileGroup.Files.Add(new File()
                            {
                                Name = file.Name,
                                Length = file.Length,
                                Type = fileType,
                                RelativePath = relativePath,
                                FtpLink = ftpLink,
                            });
                        }

                        if (fileGroup.Files.Any())
                            release.FileGroups.Add(fileGroup);

                        if (level < c_maxSearchDepth)
                        {
                            foreach (DirectoryInfo childDirectory in directory.GetDirectories())
                                ProcessDirectoryTree(level + 1, childDirectory);
                        }

                        if (level > 0)
                            pathLevels.RemoveAt(pathLevels.Count - 1);
                    }

                    ProcessDirectoryTree(0, releaseDirectory);

                    return release;
                }
                else
                    throw new InvalidOperationException("Unable to find the release directory.");
            });
        }
    }
}
