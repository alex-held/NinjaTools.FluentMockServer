using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using AutoFixture;
using FluentAssertions;
using Force.DeepCloner;
using Xunit;

namespace NinjaTools.FluentMockServer.Tests.Models
{
    public abstract class EqualityTests<T> where T : class, new()
    {
        
        protected static List<object[]> TestDataOverride { get; set; }
        
        [Fact]
        public void Equals_Should_Be_True_When_Instances_Are_Both_Default_Instances()
        {
            // Arrange
            var a = new T();
            var b = new T();
            
            // Assert
            a.Equals(b).Should().BeTrue();
        }
        
        [Fact]
        public void Equals_Should_Be_False_When_Instances_Have_Different_Types()
        {
            // Arrange
            var a = new T();
            var b = new Regex("");
            
            // Assert
            a.Equals(b).Should().BeFalse();
        }
        
        [Fact]
        public void Equals_Should_Be_True_When_Instances_Are_Not_The_Same_Reference()
        {
            // Arrange
            var a = new T();
            var b = a.DeepClone();
            
            // Assert
            a.Equals(b).Should().BeTrue();
        }

        
        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Equals_Should_Return_False_When_Instances_Are_Not_Equal(T a, object b)
        {
            // Assert
            a.Equals(b).Should().BeFalse();
        }
        
        [Theory]
        [MemberData(nameof(GetTestData))] // ReSharper disable once xUnit1026
        // ReSharper disable once RedundantAssignment
        public void Equals_Should_Return_True_When_Instances_Are_Equal(T a, object b)
        {
            // Assert
            // ReSharper disable once EqualExpressionComparison
            a.Equals(a).Should().BeTrue();
        }


        public static IEnumerable<object[]> GetTestData()
        {
            if (TestDataOverride != null)
            {
                return TestDataOverride;
            }
            
            var notEqualDataType = typeof(NotEqualData<>);
            var dataType = notEqualDataType.MakeGenericType(typeof(T));
            var dataTypeInstance = Activator.CreateInstance(dataType, null) as TheoryData<T, object>;
            // ReSharper disable once AssignNullToNotNullAttribute
            var data = dataTypeInstance!.ToList();
            return data;
        }
    }
    
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class NotEqualData<TData> : TheoryData<TData, object> where TData : class, new()
    {
        public NotEqualData()
        {
            Add(CreateRandom(), CreateRandom());
            Add(new TData(), CreateRandom());
            Add( CreateRandom(),null);
            Add(CreateRandom(), new FactAttribute());
        }

        protected virtual TData CreateRandom()
        {
            var fixture = new Fixture();

            return fixture.Create<TData>();
        }
    }
}
