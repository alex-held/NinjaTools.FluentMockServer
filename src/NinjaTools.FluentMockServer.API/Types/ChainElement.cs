namespace NinjaTools.FluentMockServer.API.Types
{
    //
    // public interface IChainElement
    // {
    //     T As<T>(T defaultValue) where T : class;
    // }
    //
    //
    // public class NullChainElement : IChainElement
    // {
    //     private static IChainElement instance;
    //     private NullChainElement() { }
    //
    //     public static IChainElement Instance = instance ??= new NullChainElement()
    //
    //
    //     /// <inheritdoc />
    //     public T As<T>(T defaultValue) where T : class
    //     {
    //         return defaultValue;
    //     }
    // }
    //
    // public interface IA
    // {
    //     string Serialize(IConfigFile file);
    //     IConfigFile Deserializer(string filepath);
    // }
    //
    //
    //
    // public class YamlSerializer : ConfigFileSerializerChainElement
    // {
    //     private SerializerBuilder SerializerBuilder { get; set; }
    //     private Func<ISerializer> BuildSerializer { get; set; }
    //
    //     private  DeserializerBuilder DeserializerBuilder { get; set; }
    //     private Func<IDeserializer> BuildDeserializer { get; set; }
    //
    //     private void Initialize()
    //     {
    //         BuildSerializer = () => SerializerBuilder.Build();
    //         SerializerBuilder = SerializerBuilder
    //             .WithMaximumRecursion(10)
    //             .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
    //             .JsonCompatible();
    //
    //         BuildDeserializer = () => DeserializerBuilder.Build();
    //         DeserializerBuilder = new DeserializerBuilder()
    //             .IgnoreUnmatchedProperties();
    //     }
    //
    //     /// <inheritdoc />
    //     public YamlSerializer(IFileSystem fileSystem, IChainElement next) : base(fileSystem, next)
    //     {
    //         Initialize();
    //     }
    //
    //     /// <inheritdoc />
    //     public YamlSerializer(IFileSystem fileSystem) : base(fileSystem)
    //     {
    //         Initialize();
    //     }
    //
    //     /// <inheritdoc />
    //     public override string Serialize(IConfigFile file)
    //     {
    //         var serializer = BuildSerializer();
    //         return serializer.Serialize(file);
    //     }
    //
    //     /// <inheritdoc />
    //     public override IConfigFile Deserializer(string filepath)
    //     {
    //         var deserializer = BuildDeserializer();
    //         return deserializer.Deserialize<IConfigFile>(filepath);
    //     }
    // }
    //
    //
    //
    //
    // public abstract class ConfigFileSerializerChainElement : ChainElement, IA
    // {
    //     private readonly IFileSystem _fileSystem;
    //
    //     public ConfigFileSerializerChainElement(IFileSystem fileSystem, IChainElement next)
    //         :base(next)
    //     {
    //         _fileSystem = fileSystem;
    //     }
    //
    //     public ConfigFileSerializerChainElement(IFileSystem fileSystem)
    //     {
    //         _fileSystem = fileSystem;
    //     }
    //
    //     /// <inheritdoc />
    //     public abstract string Serialize(IConfigFile file);
    //
    //     /// <inheritdoc />
    //     public abstract IConfigFile Deserializer(string filepath);
    // }
    //
    //
    //
    // public class ChainElement : IChainElement
    // {
    //     private readonly IChainElement? _next;
    //
    //     public ChainElement(IChainElement next)
    //     {
    //         _next = next;
    //     }
    //
    //     protected ChainElement() : this(NullChainElement.Instance)
    //     {
    //     }
    //
    //     public virtual T As<T>(T defaultValue) where T : class
    //     {
    //         if (this is T cast)
    //             return cast;
    //
    //         return _next switch
    //         {
    //             { } => _next.As(defaultValue),
    //             _ => default
    //         };
    //     }
    //
    // }
}
