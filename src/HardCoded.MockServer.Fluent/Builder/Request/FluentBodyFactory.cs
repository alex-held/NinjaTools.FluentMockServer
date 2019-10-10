using HardCoded.MockServer.Models;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Fluent.Builder
{
    public class FluentBodyBuilder : IFluentBodyBuilder
    {
        private Body Content { get; set; }

        /// <inheritdoc />
        public  IFluentBodyBuilder WithJson(string content)
        {
            Content = Body.For(Body.BodyType.JSON);
            Content.Add("json", content);
            return this;
        }

        /// <inheritdoc />
        public  IFluentBodyBuilder WithJsonItem<T>(T item)
        {
            Content = Body.For(Body.BodyType.JSON);
            Content.Add("json", JsonConvert.SerializeObject(item, Formatting.Indented));
            return this;
        }

        /// <inheritdoc />
        public  IFluentBodyBuilder MatchExactJsonContent(string content)
        {
            Content = Body.For(Body.BodyType.JSON);
            Content.Add("json", content);
            Content.Add("matchType", "STRICT");
            return this;
        }

        /// <inheritdoc />
        public  IFluentBodyBuilder WithXmlContent(string content)
        {
            Content = Body.For(Body.BodyType.XML);
            Content.Add("xml", content);
            return this;
        }

        /// <inheritdoc />
        public  IFluentBodyBuilder WithBinaryContent(byte[] content)
        {
            Content = Body.For(Body.BodyType.BINARY);
            Content.Add("binary", content);
            return this;
        }

        /// <inheritdoc />
        public IFluentBodyBuilder WithJsonArray<T>(params T[] items)
        {
            Content = Body.For(Body.BodyType.JSON);
            Content.Add("json", JsonConvert.SerializeObject(items, Formatting.Indented));
            return this;
        }

        /// <inheritdoc />
        public Body Build() =>
            Content;
    }

    public interface IFluentBodyBuilder
    {
        IFluentBodyBuilder WithJson(string content);
        IFluentBodyBuilder WithBinaryContent(byte[] content);
        IFluentBodyBuilder MatchExactJsonContent(string content);
        IFluentBodyBuilder WithXmlContent(string content);
        Body Build();
    }
}