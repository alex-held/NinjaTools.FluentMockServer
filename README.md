[![Board Status](https://dev.azure.com/alex----held/1f2ebed6-22af-4c25-93d3-fb706aa677ca/2988ffdd-29c2-4467-8dc7-7d9e5282e656/_apis/work/boardbadge/368471de-1e1a-4156-a50b-83b04b735f1c)](https://dev.azure.com/alex----held/1f2ebed6-22af-4c25-93d3-fb706aa677ca/_boards/board/t/2988ffdd-29c2-4467-8dc7-7d9e5282e656/Microsoft.RequirementCategory)
[![Board Status](https://dev.azure.com/alexander-held/00124096-a538-40ea-bac4-02a51826d901/8a0ca3cb-69d4-48e2-b635-d4aa53497c59/_apis/work/boardbadge/0f6795f5-d696-4477-b69e-273f081deda4)](https://dev.azure.com/alexander-held/00124096-a538-40ea-bac4-02a51826d901/_boards/board/t/8a0ca3cb-69d4-48e2-b635-d4aa53497c59/Microsoft.RequirementCategory)
# HardCode.MockServer

! This repository is currently under construction. !

[![Board Status](https://dev.azure.com/alexander-held/00124096-a538-40ea-bac4-02a51826d901/8a0ca3cb-69d4-48e2-b635-d4aa53497c59/_apis/work/boardbadge/0f6795f5-d696-4477-b69e-273f081deda4?columnOptions=1)](https://dev.azure.com/alexander-held/00124096-a538-40ea-bac4-02a51826d901/_boards/board/t/8a0ca3cb-69d4-48e2-b635-d4aa53497c59/Microsoft.RequirementCategory/)

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

