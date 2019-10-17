// using System;
//
// using HardCoded.MockServer.Contracts.FluentInterfaces;
// using HardCoded.MockServer.Fluent.Builder.Request;
// using HardCoded.MockServer.Fluent.Builder.Response;
//
//
// namespace HardCoded.MockServer.Fluent.Builder.Pipeline
// {
//     public interface IApplyable
//     {
//         void Apply();
//     }
//     
//     
//     public interface IAction : IAttachable<IPipeline>
//     {
//         ISetup RespondWith(Action<IFluentHttpResponseBuilder> response);
//         IAction Forward(Action<object> forward);
//         IAction Error();
//     }
//
//     public interface IPipeline : IApplyable
//     {
//         ISetup Setup { get; }
//         
//         IState State { get; }
//
//         IVerify Verify { get; }
//     }
//
//     public interface IState : IAttachable<IPipeline>
//     {
//         void Reset();
//         
//         IState ClearSetups<T>(Action<IFluentHttpRequestBuilder> setups);
//         IState ClearLogs<T>(Action<IFluentHttpRequestBuilder> logs);
//         IState ClearMatching<T>(Action<IFluentHttpRequestBuilder> match);
//     }
//
//     public interface IVerify
//     {
//         IVerify Request();
//         IVerify Sequence();
//     }
//     
//     public interface ISetup : IAttachable<IPipeline>
//     {
//         IAction WhenHandling(Action<IFluentHttpRequestBuilder> request);
//     }
//
//     
//
//     public class Pipeline
//     {
//         private IPipeline pipeline { get; }
//
//
//         public Pipeline()
//         {
//             pipeline
//                         .Setup
//                         .WhenHandling(
//                             request => request
//                                         .WithPath("test")
//                                         .EnableEncryption()
//                                         .KeepConnectionAlive()
//                                         .WithContent(c => c.ContainingJson("")))
//                         .RespondWith(
//                             response => response
//                                         .WithBody(b => b.ContainingJson(""))
//                                         .WithDelay(d => d.FromMiliSeconds(1000))
//                                         .WithConnectionOptions(opt => opt.Build()));
//         }
//     }
// }
