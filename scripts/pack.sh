#!/usr/bin/env bash

project="/home/vsts/work/1/s/src/NinjaTools.FluentMockServer.TestContainers/NinjaTools.FluentMockServer.TestContainers.csproj"
output="/home/vsts/work/1/a/packages"
version="20200105.2"

dotnet pack $project --output $output /p:VersionSuffix=$version --verbosity Detailed
