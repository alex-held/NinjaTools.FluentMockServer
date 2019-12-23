using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers.Data.Random
{
    internal class InstanceFactoryCreator
    {
        public static T CreateDefault<T>() => CreateFactory<T>().Invoke();

        public static T CreateRandom<T>()
        {
            var parameterTypes = GetConstructorParameterTypes<T>();
            var constructor = ResolveBestConstructor<T>();
            var parameters = parameterTypes
                .Select(t =>
                {
                    var fixture = new Fixture();
                    var context = new SpecimenContext(fixture);
                    var randomParameterValue = context.Resolve(t);
                    return randomParameterValue;
                })
                .ToArray();

            // ReSharper disable once SuggestVarOrType_SimpleTypes
            T instance = (T) constructor.Invoke(parameters); 
            return  instance;
        }
        
        public static IEnumerable<Type> GetConstructorParameterTypes<T>()
        {
            var dataType = typeof(T);
            var constructors = dataType
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(p => p.GetParameters().Length);

            var ctorWithLeastParameters = constructors.First();

            return ctorWithLeastParameters
                .GetParameters()
                .Select(pi => pi.ParameterType);
        }
        
        
        public static Func<TData> CreateFactory<TData>() 
        {
            try
            {
                var defaultInstance = Activator.CreateInstance(typeof(TData));
                return () => (TData) defaultInstance;
            }
            catch
            {
                var constructor = ResolveBestConstructor<TData>();
                var constructorParamInfos = constructor.GetParameters();
                var constructorParams = constructorParamInfos
                    .Select(pi => pi.ParameterType.IsValueType
                        ? Activator.CreateInstance(pi.ParameterType)
                        : null)
                    .ToArray();
                
                // var instance = Activator.CreateInstance(dataType,bindingFlags , constructorParams);
                var instance = (TData)  constructor.Invoke(constructorParams.ToArray());
                return () => instance ;
            }
        }

        public static ConstructorInfo ResolveBestConstructor<TData>() 
        {
            var dataType = typeof(TData);
            var constructors = dataType
                .GetConstructors()
                .OrderBy(p => p.GetParameters().Length);

            var ctorWithLeastParameters = constructors.First();
            
            return ctorWithLeastParameters;
        }
    }
}
