using AnimeLibraryBrowser.Configuration;
using AnimeLibraryBrowser.Services;
using AnimeLibraryBrowser.Services.Interfaces;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AnimeLibraryBrowser
{
    public class Startup
    {
        private const string c_clientApplicationPath = "ClientApplication";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AnimeLibraryConfiguration>(Configuration.GetSection(nameof(AnimeLibraryConfiguration)));
            services.Configure<FtpLinkFormatterConfiguration>(Configuration.GetSection(nameof(FtpLinkFormatterConfiguration)));

            services.AddMemoryCache();

            services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

            services.AddSingleton<IDirectoryTreeAnalyzer, DirectoryTreeAnalyzer>();
            services.AddSingleton<IReleaseDetailsProvider, ReleaseDetailsProvider>();
            services.AddSingleton<IReleaseDirectorySelector, ReleaseDirectorySelector>();
            services.AddSingleton<IFileTypeResolver, FileTypeResolver>();

            services.AddSwaggerGen();
            services.AddControllersWithViews();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = $"{c_clientApplicationPath}/build";
            });
        }

        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                applicationBuilder.UseSwagger();
                applicationBuilder.UseSwaggerUI();
                applicationBuilder.UseDeveloperExceptionPage();
            }
            else
            {
                applicationBuilder.UseExceptionHandler("/Error");
                applicationBuilder.UseHsts();
            }

            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseStaticFiles();
            applicationBuilder.UseSpaStaticFiles();

            applicationBuilder.UseRouting();

            applicationBuilder.UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapControllers();
            });

            applicationBuilder.UseSpa(spaBuilder =>
            {
                spaBuilder.Options.SourcePath = c_clientApplicationPath;

                if (webHostEnvironment.IsDevelopment())
                {
                    spaBuilder.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
