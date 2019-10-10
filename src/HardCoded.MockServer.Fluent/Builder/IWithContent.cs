using FluentApi.Generics.Framework;

namespace HardCoded.MockServer.Fluent.Builder
{
    public interface IWithContent<TBuildable> : IFluentInterface
    {
        TBuildable WithJsonContent(string content);
        TBuildable WithXmlContent(string content);
        TBuildable WithBinaryContent(byte[] content);
        TBuildable WithJsonArray<T>(params T[] items);
        TBuildable WithJson<T>(T item);
        TBuildable MatchExactJsonContent(string content);
    }
}