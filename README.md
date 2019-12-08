# HardCode.MockServer

! This repository is currently under construction. !

[![Build Status](https://dev.azure.com/alex-held/NinjaTools.FluentMockServer/_apis/build/status/alex-held.NinjaTools.FluentMockServer?branchName=master)](https://dev.azure.com/alex-held/NinjaTools.FluentMockServer/_build/latest?definitionId=2&branchName=master) [![Board Status](https://dev.azure.com/alex-held/1d2716aa-e492-484f-9771-a7ae71f6f3fb/c4f6723f-2182-4ab9-979a-9011eda004f8/_apis/work/boardbadge/a9799606-87b5-4e95-b7e3-2cec9c84c163?columnOptions=1)](https://dev.azure.com/alex-held/1d2716aa-e492-484f-9771-a7ae71f6f3fb/_boards/board/t/c4f6723f-2182-4ab9-979a-9011eda004f8/Microsoft.RequirementCategory/)

## What are we trying to solve?

This library is a dotnet client around the fabulous [MockServer](https://www.mock-server.com/) docker image.

We will provide an easy and on-the-fly way to setup all sorts of HttpServers with Responses, Timeouts, Exceptions and so on. 


## Quickstart

### Setting up Expectations:

``` csharp

MockServerSetup.Expectations
                    .OnHandling(HttpMethod.Delete,
                                request =>
                                    request
                                    .WithPath("post")
                                    .EnableEncryption()
                                    .KeepConnectionAlive()
                    )
                    .RespondWith(HttpStatusCode.Accepted,
                                response => response
                                    .WithBody(content => content.WithJson(""))
                    )
                    .And
                    .OnHandling(HttpMethod.Delete,
                                request =>
                                    request
                                    .WithPath("post")
                                    .EnableEncryption()
                                    .KeepConnectionAlive()
                    )
                    .RespondWith(HttpStatusCode.Accepted,
                                response => response
                                    .WithBody(content => content.WithJson(""))
                    ).Setup();


```


## Extensions

Currently we support:

- [testcontainers-dotnet](https://github.com/testcontainers/testcontainers-dotnet) to spin up a MockServer docker container from within an [xunit]() test.

- [xunit]()

