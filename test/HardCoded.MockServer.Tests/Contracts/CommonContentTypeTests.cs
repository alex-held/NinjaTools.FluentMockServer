using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Bogus.DataSets;

using FluentAssertions;

using HardCoded.MockServer.Contracts;
using HardCoded.MockServer.Contracts.Serialization;

using Xunit;
using Xunit.Abstractions;

#nullable enable
namespace HardCoded.MockServer.Tests.Contracts
{

    public class RandomDataGenerator
    {
        public static Dictionary<Encyclopedia, CommonContentType> GetRandomEncyclopedia(int min, int max)
        {
            var random = new Bogus.Randomizer();
            var lorem = new Lorem();
            var quantity = random.Number(min, max);
            var sentences = random.Number(1, 20);
            var entries = random.Int(1, 200);
            
            
            Func<int, int> GetVariance = (i) => i + random.Int(0, 3);
            var randomTestData = new Dictionary<Encyclopedia, CommonContentType>();
            
            for ( var i = 0; i < quantity; i++ ) {
                var encyclopedia  = new Encyclopedia()
                { Title = lorem.Sentence(), Eintrag = new List<Entry>(CreateEntries())};
                randomTestData.Add(encyclopedia, random.Bool() ? CommonContentType.Json : CommonContentType.Xml);
            }

            return randomTestData;
            
            IEnumerable<Entry> CreateEntries()
            {
                for ( var i = 0; i < GetVariance(entries); i++ ) {
                    yield return new Entry
                    {
                        Eintragstext = lorem.Sentence(GetVariance(sentences)), 
                        Stichwort = lorem.Word()
                    };
                }
            }
        }
    }
    
    /// <inheritdoc />
    public class SerializationTestData : TheoryData<Encyclopedia, CommonContentType, string, byte[]>
    {
        [Pure][return: MaybeNull]
        private byte[]? EncodeXml<T>(T body, Encoding encoding, out string xml)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter);
            xmlSerializer.Serialize(xmlWriter, body);
            xml = stringWriter.ToString();
            return encoding.GetBytes(xml);
        }
        
        public SerializationTestData()
        {
            var dict = RandomDataGenerator.GetRandomEncyclopedia(10, 50);
            
            foreach ( var keyValue in dict ) {
                var bytes = EncodeXml(keyValue.Key, Encoding.UTF8, out var xml);
                Add(keyValue.Key, CommonContentType.Xml, xml, bytes);
            }
        }
    }
    
    
    public class Encyclopedia
    {
        public string Title { get; set; }
        public List<Entry> Eintrag { get; set; }
    }

        
    public class Entry
    {
        public string Stichwort { get; set; }
        public string Eintragstext { get; set; }
    }

    
    public class CommonContentTypeTests
    {
        private readonly ITestOutputHelper _output;
        public CommonContentTypeTests(ITestOutputHelper output) => _output = output; 


        [Fact]
        public void Should_Provide_All_Registered_ContentTypes()
        {
            // Arrange
            var expected = new List<CommonContentType>()
            {
                        CommonContentType.Html
                      , CommonContentType.Json
                      , CommonContentType.Plain
                      , CommonContentType.Soap12
                      , CommonContentType.Xml
            };

            // Act
            var list = CommonContentType.ToList();

            // Assert
            list.Should().Contain(expected);
        }
        
        
        [Pure]
        [return: MaybeNull]
        private byte[] EncodeXml<T>(T body, Encoding encoding, out string xml)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter);
            xmlSerializer.Serialize(xmlWriter, body);
            xml = stringWriter.ToString();
            var bytes = encoding.GetBytes(xml);
            _output.WriteLine($"Bytes: {bytes.Length}");
            _output.WriteLine(xml);

            return bytes;
        }

        
        [Theory]
        [ClassData(typeof(SerializationTestData))]
        public void Should_Serialize_All_Kinds_Of_Random_Test_Data(Encyclopedia encyclopedia, CommonContentType contentType, string text, byte[] bytes)
        {
            // Arrange
            _output.WriteLine($"ContentType: {contentType}; bytes: {bytes.Length}"); 
            _output.WriteLine($"Text: {text}");

            // Act
            var actualBytes = EncodingUtils<Encyclopedia>.Encode(encyclopedia, contentType, Encoding.UTF8);
            
            // Assert
            actualBytes.Length.Should().Be(bytes.Length, "since both byte arrays should contain the same data.");
  //          actualBytes.SequenceEqual(bytes).Should().BeTrue("since both byte arrays should contain the same data.");
        }
    }
}
