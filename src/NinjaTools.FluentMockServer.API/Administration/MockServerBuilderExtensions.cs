using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NinjaTools.FluentMockServer.API.DependencyInjection;

namespace NinjaTools.FluentMockServer.API.Administration
{
    public interface IAdminPath
    {
        string Path { get; }
        public bool IsAdminPath(string path) => IsAdminPath(new PathString(Path));
        public bool IsAdminPath(PathString pathString) => pathString.StartsWithSegments(Path);
        public bool IsAdminPath(Uri uri) => IsAdminPath(uri.ToString());
    }

    internal class AdminPath : IAdminPath
    {
        public AdminPath(string path)
        {
            Path = $"/{path.TrimStart('/')}";
        }

        /// <inheritdoc />
        public string Path { get; }
    }

    public static class MockServerBuilderExtensions
    {
        public static IMockServerBuilder AddAdminPath(this IMockServerBuilder builder, string path)
        {
            builder.Services.TryAddSingleton<IAdminPath>(new AdminPath(path));
            return builder;
        }
    }
}
