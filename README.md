# HardCode.MockServer

! This repository is currently under construction. !

## Status
[![azure-pipeline](https://dev.azure.com/aheldext/HardCode.MockServer/_apis/build/status/alex-held.HardCode.MockServer?branchName=master)](https://dev.azure.com/aheldext/HardCode.MockServer/_build/latest?definitionId=4&branchName=master)

[![Board Status](https://dev.azure.com/aheldext/dd2a5c29-0864-43a2-acde-a0a3da18fe71/b512aad6-fd76-4d19-86c7-fbda79cb29c2/_apis/work/boardbadge/8f972b26-27fd-4f20-922a-023bdc291a3c?columnOptions=1)](https://dev.azure.com/aheldext/dd2a5c29-0864-43a2-acde-a0a3da18fe71/_boards/board/t/b512aad6-fd76-4d19-86c7-fbda79cb29c2/Microsoft.RequirementCategory/)

## What are we trying to solve?

This library is a dotnet client around the fabulous [MockServer](https://www.mock-server.com/) docker image.

We will provide an easy and on-the-fly way to setup all sorts of HttpServers with Responses, Timeouts, Exceptions and so on. 


## Extensions

Currently we support [testcontainers-dotnet](https://github.com/testcontainers/testcontainers-dotnet) to spin up a MockServer docker container from within an [xunit]() test.
