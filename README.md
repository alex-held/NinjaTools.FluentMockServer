# NinjaTools.FluentMockServer

**Easy integration testing:** For an overview of the development workflow see
[DEVELOPMENT.md](https://github.com/alex-held/NinjaTools.FluentMockServer/blob/aphrodite/DEVELOPMENT.md).

FluentMockServer is an heavily inspired by providing additional tooling for
setting up the MockServer via c# code.

The project was inspired by other MockServers such as
[mockserver](https://github.com/jamesdbloom/mockservice).

See the Roadmap blogpost for an overview of planned features.

## Getting Started

[![Installation](https://img.shields.io/badge/wiki-installation-brightgreen.svg?maxAge=60&style=flat-square)](https://github.com/alex-held/NinjaTools.FluentMockServer/wiki/Installation)
[![FAQ](https://img.shields.io/badge/wiki-FAQ-BF55EC.svg?maxAge=60&style=flat-square)](https://github.com/alex-held/NinjaTools.FluentMockServer/wiki/FAQ)

## Workflows

You can choose between or combine different mode of operations

1. Run an instance of
   [mockserver](https://hub.docker.com/r/jamesdbloom/mockserver) docker
   container in the background
2. Start an instance of
   [mockserver](https://hub.docker.com/r/jamesdbloom/mockserver) docker
   container on demand in your c# code

[Try it out](https://gitpod.io/#https://github.com/alex-held/NinjaTools.FluentMockServer)
in the browser!

## Support

[![Board Status](https://dev.azure.com/alex-held/1f2ebed6-22af-4c25-93d3-fb706aa677ca/2988ffdd-29c2-4467-8dc7-7d9e5282e656/_apis/work/boardbadge/368471de-1e1a-4156-a50b-83b04b735f1c?columnOptions=1)](https://dev.azure.com/alex-held/1f2ebed6-22af-4c25-93d3-fb706aa677ca/_boards/board/t/2988ffdd-29c2-4467-8dc7-7d9e5282e656/Microsoft.RequirementCategory/)

[![GitHub](https://img.shields.io/badge/github-issues-red.svg?maxAge=60&style=flat-square)](https://github.com/alex-held/NinjaTools.FluentMockServer/issues)
[![GitHub Wiki](https://img.shields.io/badge/github-wiki-181717.svg?maxAge=60&style=flat-square)](https://github.com/alex-held/NinjaTools.FluentMockServer)
![GitHub pull requests](https://img.shields.io/github/issues-pr/alex-held/NinjaTools.FluentMockServer?logo=GitHub&style=flat-square)

## Status

![Azure DevOps builds (master)](https://img.shields.io/azure-devops/build/alex-held/1f2ebed6-22af-4c25-93d3-fb706aa677ca/5/master?style=flat-square)
![Azure DevOps builds (develop)](https://img.shields.io/azure-devops/build/alex-held/1f2ebed6-22af-4c25-93d3-fb706aa677ca/5/develop?style=flat-square)

[![CodeFactor](https://www.codefactor.io/repository/github/alex-held/ninjatools.fluentmockserver/badge)](https://www.codefactor.io/repository/github/alex-held/ninjatools.fluentmockserver)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/e0e21ac86f80480d8a806b98acb57b0f)](https://www.codacy.com/manual/git_36/NinjaTools.FluentMockServer?utm_source=github.com&utm_medium=referral&utm_content=alex-held/NinjaTools.FluentMockServer&utm_campaign=Badge_Grade)
[![Gitpod Ready-to-Code](https://img.shields.io/badge/Gitpod-Ready--to--Code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/alex-held/NinjaTools.FluentMockServer)
![GitHub commit activity](https://img.shields.io/github/commit-activity/w/alex-held/NinjaTools.FluentMockServer?color=bright-green&label=Changelog&logo=GitHub)

[![Copyright 2019-2020](https://img.shields.io/badge/copyright-2019-blue.svg?maxAge=60&style=flat-square)](https://github.com/alex-held/FluentMockServer)
[![GNU GPL v3](https://img.shields.io/badge/license-GNU%20GPL%20v3-blue.svg?maxAge=60&style=flat-square)](http://www.gnu.org/licenses/gpl.html)

## Releases

| Package                                    |                                                         Master                                                          |                                                          Develop                                                           |
| ------------------------------------------ | :---------------------------------------------------------------------------------------------------------------------: | :------------------------------------------------------------------------------------------------------------------------: |
| NinjaTools.FluentMockServer                | ![NinjaTools.FluentMockServer](https://img.shields.io/nuget/v/NinjaTools.FluentMockServer?logo=nuget&style=flat-square) | ![NinjaTools.FluentMockServer](https://img.shields.io/nuget/vpre/NinjaTools.FluentMockServer?logo=nuget&style=flat-square) |
| NinjaTools.FluentMockServer.TestContainers |    ![Nuget](https://img.shields.io/nuget/v/NinjaTools.FluentMockServer.TestContainers?logo=nuget&style=flat-square)     |    ![Nuget](https://img.shields.io/nuget/vpre/NinjaTools.FluentMockServer.TestContainers?logo=nuget&style=flat-square)     |

| Package         |                                                                              Master                                                                               |                                                                               Develop                                                                               |
| --------------- | :---------------------------------------------------------------------------------------------------------------------------------------------------------------: | :-----------------------------------------------------------------------------------------------------------------------------------------------------------------: |
| Azure Pipelines | ![Azure DevOps builds (branch)](https://img.shields.io/azure-devops/build/alex-held/1f2ebed6-22af-4c25-93d3-fb706aa677ca/6/master?label=master&style=flat-square) | ![Azure DevOps builds (branch)](https://img.shields.io/azure-devops/build/alex-held/1f2ebed6-22af-4c25-93d3-fb706aa677ca/6/develop?label=develop&style=flat-square) |

NinjaTools.FluentMockServer is currently maintained and pull requests are
actively added into the repository.

## Features

### Current Features

- Tightly integrated with
  [mockserver](https://github.com/jamesdbloom/mockservice)
- In-memory creation of MockServer Docker Instances with
  [FluentDocker](https://github.com/mariotoffia/FluentDocker)
- [XUnit](https://github.com/xunit/xunit) integration with Fixtures
- Configure an `Expectation`:
  - Select which `HttpRequest` to handle
  - Configure an action like `Exceptions`, `HttpResponse`, `Timeout`,
    `ConnectionLos` ...
  - _Optionally_ provide for how often / timespan the expectation is valid
- Verfiy `Expectation`s:
  - Select which `HttpRequest` to verify
  - Verify if has been matched
  - Verify how often

### Planned Features

- Admin UI
- Configuration via json/yaml files
- Support for var

### Requirements

- [Visual Studio Community 2019](https://www.visualstudio.com/vs/community/) or
  [Rider](http://www.jetbrains.com/rider/)
- [Git](https://git-scm.com/downloads)
- [Docker](https://docker.com)
