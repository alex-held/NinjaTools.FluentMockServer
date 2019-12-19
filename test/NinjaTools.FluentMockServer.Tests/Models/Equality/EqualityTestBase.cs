#nullable enable
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentAssertions;
using Force.DeepCloner;
using JetBrains.Annotations;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using NinjaTools.FluentMockServer.Tests.TestHelpers.Data.Random;
using NinjaTools.FluentMockServer.Tests.TestHelpers.Data.TheoryData;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public abstract class EqualityTestBase<T> : XUnitTestBase<T> where T : class
    {
        // ReSharper disable once StaticMemberInGenericType
        protected static List<object[]> TestDataOverride { get; set; }
        protected EqualityTestBase(ITestOutputHelper outputHelper) : base(outputHelper)
        { }
        
        [Theory]
        [MemberData(nameof(GetEqualityOperatorTestData))]
        public void Equals_Should_Compare_For_Equality_By_Properties([NotNull] T a, [NotNull] T b, bool areEqual)
        {
            // Assert
            switch (areEqual)
            {
                case true:
                    a.Should().Be(b);
                    break;
                default:
                    a.Should().NotBe(b);
                    break;
            }
        }

        [Fact]
        public void Equals_Should_Be_False_When_Instances_Have_Different_Types()
        {
            // Arrange
            var a = InstanceFactoryCreator.CreateDefault<T>();
            var b = new Regex("");

            // Assert
            a.Equals(b).Should().BeFalse();
        }

        [Fact]
        public void Equals_Should_Be_True_When_Instances_Are_Not_The_Same_Reference_But_Have_Same_Properties()
        {
            // Arrange
            var a = InstanceFactoryCreator.CreateDefault<T>();
            var b = a.DeepClone();

            // Assert
            a.Equals(b).Should().BeTrue();
        }


        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Equals_Should_Return_False_When_Instances_Are_Not_Equal([NotNull] T a, [CanBeNull] object? b)
        {
            Output.WriteLine($"{nameof(a)}: {JsonConvert.SerializeObject(a, Formatting.Indented)}; Type={typeof(T).Name};\n{nameof(b)}: {b}; Type={b?.GetType().Name ?? "<null>"};\n");
            
            // Assert
            a.Should().NotBe(b);
        }

        [Theory]
        [MemberData(nameof(GetTestData))] 
        public void Equals_Should_Return_True_When_Instances_Are_Equal([NotNull] T a, [CanBeNull] object? b)
        {
            Output.WriteLine($"{nameof(a)}: {a}; Type={typeof(T).Name};\n{nameof(b)}: {b}; Type={b?.GetType().Name ?? "<null>"};\n");
           
            // Assert
            // ReSharper disable once EqualExpressionComparison
            a.Equals(a).Should().BeTrue();
        }

      
        [Theory]
        [MemberData(nameof(GetEqualityOperatorTestData))]
        public void EqualityOperators_Should_Compare_By_Equality_And_Not_By_Reference([NotNull] T a, [NotNull] T b, bool areEqual)
        {
            Output.WriteLine($"{nameof(a)}: {JsonConvert.SerializeObject(a, Formatting.Indented)}; Type={a.GetType().Name};\n{nameof(b)}: {JsonConvert.SerializeObject(b, Formatting.Indented)}; Type={b.GetType().Name};\n");
            
            // Act
            var equalResult = a == b;
            var unEqualResult = a != b;
            
            // Assert
            equalResult.Should().Be(areEqual);
            unEqualResult.Should().Be(!areEqual);
        }
        
        [Theory]
        [MemberData(nameof(GetEqualityOperatorTestData))]
        public void GetHashCode_Should_Be_Correct([NotNull] T a, [NotNull] T b, bool areEqual)
        {
            var hashA = a.GetHashCode();
            var hashB = b.GetHashCode();
            
            Output.WriteLine($"hash of {nameof(a)}: {hashA}; Type={typeof(T).Name};\nhash of {nameof(b)}: {hashB}; Type={b.GetType().Name};\n");
          
            // Assert
            if (areEqual)
            {
               hashA.Should().Be(hashB);
            }
            else
            {
                hashA.GetHashCode().Should().NotBe(hashB);
            }
        }

        [NotNull]
        public static IEnumerable<object[]> GetEqualityOperatorTestData()
        {
            var equalityOperatorDataType = typeof(EqualityOperatorTestData<>);
            var dataType =  equalityOperatorDataType.MakeGenericType(typeof(T));
            var dataTypeInstance = Activator.CreateInstance(dataType) as EqualityOperatorTestData<T>;
            return dataTypeInstance ?? throw new ArgumentNullException(nameof(dataTypeInstance));
        }
        
        
        [NotNull]
        public static IEnumerable<object[]> GetTestData()
        {
            if (TestDataOverride != null)
            {
                return TestDataOverride;
            }
            var data = GetTheoryData();
            return data;
        }

        [NotNull]
        private static NotEqualData<T> GetTheoryData()
        {
            var notEqualDataType = typeof(NotEqualData<>);
            var dataType = notEqualDataType.MakeGenericType(typeof(T));
            var dataTypeInstance = Activator.CreateInstance(dataType) as NotEqualData<T>;
            return dataTypeInstance ?? throw new ArgumentNullException(nameof(dataTypeInstance));
        }
    }
}
