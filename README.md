# HardCode.MockServer

! This repository is currently under construction. !

## Status
![azure-pipelines](https://dev.azure.com/aheldext/HardCode.MockServer/_apis/build/repos/github/badge?api-version=5.1-preview.2)

## What are we trying to solve?

This library is a dotnet client around the fabulous [MockServer](https://www.mock-server.com/) docker image.

We will provide an easy and on-the-fly way to setup all sorts of HttpServers with Responses, Timeouts, Exceptions and so on. 


## Extensions

Currently we support [testcontainers-dotnet](https://github.com/testcontainers/testcontainers-dotnet) to spin up a MockServer docker container from within an [xunit]() test.
