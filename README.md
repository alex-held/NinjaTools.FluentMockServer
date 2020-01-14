[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-black.svg)](https://sonarcloud.io/dashboard?id=alex-held_NinjaTools.FluentMockServer)

![Azure DevOps builds (branch)](https://img.shields.io/azure-devops/build/alex-held/1f2ebed6-22af-4c25-93d3-fb706aa677ca/5/master?label=master&logo=git&style=for-the-badge)
![Azure DevOps builds (develop)](https://img.shields.io/azure-devops/build/alex-held/1f2ebed6-22af-4c25-93d3-fb706aa677ca/5/develop?label=develop&logo=git&style=for-the-badge)

![Azure DevOps tests (branch)](https://img.shields.io/azure-devops/tests/alex-held/NinjaTools.FluentMockServer/4/develop?logo=azure-pipelines&logoColor=white&style=for-the-badge)

---

![Nuget (stable)](https://img.shields.io/nuget/v/NinjaTools.FluentMockServer?label=stable&logo=nuget&style=for-the-badge)

![Nuget](https://img.shields.io/nuget/vpre/NinjaTools.FluentMockServer?label=Prerelease&logo=nuget&style=for-the-badge)

---

![Sonar Tech Debt](https://img.shields.io/sonar/tech_debt/alex-held_NinjaTools.FluentMockServer?server=https%3A%2F%2Fsonarcloud.io&logo=sonarcloud&style=for-the-badge)
![Sonar Coverage](https://img.shields.io/sonar/coverage/alex-held_NinjaTools.FluentMockServer?server=https%3A%2F%2Fsonarcloud.io&logo=sonarcloud&style=for-the-badge)
![CodeFactor Grade](https://img.shields.io/codefactor/grade/github/alex-held/NinjaTools.FluentMockServer?logo=codefactor&style=for-the-badge)
![Codacy grade](https://img.shields.io/codacy/grade/e0e21ac86f80480d8a806b98acb57b0f?color=brightgreen&logo=codacy&style=for-the-badge)


[![Gitpod Ready-to-Code](https://img.shields.io/badge/gitpod-Ready%20to%20code-blue?logo=gitpod&style=for-the-badge)](https://gitpod.io/#https://github.com/alex-held/NinjaTools.FluentMockServer)

---
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/alex-held/NinjaTools.FluentMockServer?logo=when-i-work&style=for-the-badge&logoColor=white)


[![GitHub](https://img.shields.io/badge/github-issues-red.svg?maxAge=60&style=for-the-badge)](https://github.com/alex-held/NinjaTools.FluentMockServer/issues)
[![GitHub Wiki](https://img.shields.io/badge/github-wiki-181717.svg?maxAge=60&style=for-the-badge)](https://github.com/alex-held/NinjaTools.FluentMockServer)
![GitHub pull requests](https://img.shields.io/github/issues-pr/alex-held/NinjaTools.FluentMockServer?logo=GitHub&style=for-the-badge)

---

[![Copyright 2019-2020](https://img.shields.io/badge/copyright-2019-blue.svg?maxAge=60&style=for-the-badge)](https://github.com/alex-held/FluentMockServer)
[![GNU GPL v3](https://img.shields.io/badge/license-GNU%20GPL%20v3-blue.svg?maxAge=60&style=for-the-badge)](http://www.gnu.org/licenses/gpl.html)


# NinjaTools.FluentMockServer

FluentMockServer is an heavily inspired by providing additional tooling for
setting up the MockServer via code.

The project was inspired by other MockServers such as
[mockserver](https://github.com/jamesdbloom/mockservice).

See the [ROADMAP.md](https://github.com/alex-held/NinjaTools.FluentMockServer/blob/master/ROADMAP.md) for an overview of planned features.

NinjaTools.FluentMockServer is currently maintained and pull requests are actively added into the repository.


## Getting Started

[![Installation](https://img.shields.io/badge/wiki-installation-brightgreen.svg?maxAge=60&style=for-the-badge)](https://github.com/alex-held/NinjaTools.FluentMockServer/wiki/Installation)
[![FAQ](https://img.shields.io/badge/wiki-FAQ-BF55EC.svg?maxAge=60&style=for-the-badge)](https://github.com/alex-held/NinjaTools.FluentMockServer/wiki/FAQ)

You can choose between or combine different mode of operations

1. Run an instance of
   [mockserver](https://hub.docker.com/r/jamesdbloom/mockserver) docker
   container in the background
2. Start an instance of
   [mockserver](https://hub.docker.com/r/jamesdbloom/mockserver) docker
   container on demand in your c# code

[Try it out](https://gitpod.io/#https://github.com/alex-held/NinjaTools.FluentMockServer)
in the browser!

[![Open in Gitpod](https://gitpod.io/button/open-in-gitpod.svg)](https://gitpod.io/#https://github.com/alex-held/NinjaTools.FluentMockServer)


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

### How to contribute

An contibution is welcome. For more information check out [CONTRIBUTING.md](https://github.com/alex-held/NinjaTools.FluentMockServer/blob/master/CONTRIBUTING.md).

If you have any questions feel free to contact me or join our [Discord](https://discord.gg/NHSgRyx).

### Requirements

- [Visual Studio Community 2019](https://www.visualstudio.com/vs/community/) or
  [Rider](http://www.jetbrains.com/rider/)
- [Git](https://git-scm.com/downloads)
- [Docker](https://docker.com)
