using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using NinjaTools.FluentMockServer.Tests.TestHelpers.Data.Random;
using Xunit;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers.Data.TheoryData
{
    public sealed class NotEqualData<TData> : TheoryData<TData, object>
    {
        public NotEqualData()
        {
            Add(CreateRandom(), CreateRandom());
            Add(InstanceFactoryCreator.CreateDefault<TData>(), CreateRandom());
            Add(CreateRandom(), null);
            Add(CreateRandom(), new FactAttribute());
        }

        public TData CreateRandom()
        {
            var parameterTypes = InstanceFactoryCreator.GetConstructorParameterTypes<TData>();
            var constructor = InstanceFactoryCreator.ResolveBestConstructor<TData>();
            var parameters = parameterTypes
                .Select(t =>
                {
                    var fixture = new Fixture();
                    var context = new SpecimenContext(fixture);
                    var randomParameterValue = context.Resolve(t);
                    return randomParameterValue;
                })
                .ToArray();
            
            var instance = constructor.Invoke(parameters);
            return (TData) instance;
        }
    }
}
