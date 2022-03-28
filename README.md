# Anime Library Browser

A web interface for navigating and using local or LAN-accessible anime library.

## Dependencies

This project utilizes the following frameworks, libraries, and technologies:

- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) and [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) for cross-platform server-side code.
- [TypeScript](https://www.typescriptlang.org/) and [React](https://reactjs.org/) for client-side code.
- [Bootstrap](https://getbootstrap.com/) for UI components library.

## Installation Guide

You should have Node.js, .NET Core 3.1 SDK and ASP.NET Core 3.1 Runtime on your machine.

Installing dependencies (starting in the root folder of the repository):

    cd Source
    dotnet restore .\AnimeLibraryBrowser.sln
    dotnet build .\AnimeLibraryBrowser.sln

Running unit tests:

    dotnet test .\AnimeLibraryBrowser.sln

Starting the web application:

    cd AnimeLibraryBrowser
    dotnet run .\AnimeLibraryBrowser.csproj

## Configuration

Description of application-specific configuration keys found in `appsettings.json`:

|            Section            |              Key               |                           Description                           |        Example Value        |
| :---------------------------: | :----------------------------: | :-------------------------------------------------------------: | :-------------------------: |
|   AnimeLibraryConfiguration   |         RootDirectory          |           Path to the root directory of the library.            |        "D:\\\\Anime"        |
|   AnimeLibraryConfiguration   |    ReleaseDirectoryTemplate    |         Regular expression for parsing release details.         |   Check the next chapter.   |
|   AnimeLibraryConfiguration   | ReleaseDirectorySearchTemplate | Regular expression for finding a release directory by its name. |   Check the next chapter.   |
|   AnimeLibraryConfiguration   |      VideoFileExtensions       |               List of extensions for video files.               |     [ ".mkv", ".avi" ]      |
|   AnimeLibraryConfiguration   |      AudioFileExtensions       |               List of extensions for audio files.               |     [ ".mka", ".aac" ]      |
|   AnimeLibraryConfiguration   |    SubtitlesFileExtensions     |       List of extensions for files containing subtitles.        |     [ ".ass", ".srt" ]      |
| FtpLinkFormatterConfiguration |      RootDirectoryFtpLink      |         FTP-path to the root directory of the library.          | "ftp://192.168.1.16/Anime/" |

## Regular Expressions

There are two regular expressions, the first one is for parsing release details from directory names and the second one is for finding a directory by a provided release name.

The suggested value for the `AnimeLibraryConfiguration.ReleaseDirectoryTemplate` configuration key is provided below:

    (?'Title'.+)\\s\\((?'Year'\\d{4})\\)\\s\\[(?'Type'.+),\\s(?'FrameWidth'\\d{1,5})x(?'FrameHeight'\\d{1,5}),\\s(?'VideoEncoder'.+),\\s(?'AudioEncoder'.+)\\]

It has the following capture groups: `Title`, `Year`, `Type`, `FrameWidth`, `FrameHeight`, `VideoEncoder`, `AudioEncoder`. The first one is required, the rest of them are optional.
The provided regular expression will parse release details from directories named in the following style:

    Angel Beats! (2010) [BDRip, 1920x1080, AVC, FLAC]
    I Want to Eat Your Pancreas (2018) [BDRip, 1920x1080, HEVC, FLAC]
    Violet Evergarden (2018) [BDRip, 1920x1080, AVC, FLAC]
    Your Lie in April (2015) [BDRip, 1920x1080, AVC, FLAC]
    Your Name (2017) [BDRip, 1920x1080, AVC, FLAC]

There is also the `AnimeLibraryConfiguration.ReleaseDirectorySearchTemplate` configuration key that contains a formattable string.
It means that all regex tokens that represent placeholders for string formatting should be escaped with extra braces.
The suggested value is:

    {0}\\s\\(\\d{{4}}\\)\\s\\[.+\\]

Here `{0}` is a placeholder for the release name and `\\d{{4}}` is an escaped regex token that would be processed as the second placeholder if it was not wrapped in extra braces (`{}`).
