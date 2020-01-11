# Contributing to NinjaTools.MockServer

## Branching

When creating a `feature` branch, branch from `develop` and name it `feature/my-feature`.

Regulary check for updates on `develop` and rebase / merge upstream changes into your `feature`.

Create a pull-request, `merging` from `feature/my-feature` into `develop`.

## Checklist before creating a pull request

- Squash commits Often we create temporary commits like "Started implementing feature x" and then "Did a bit more on feature x". Squash these commits together using interactive rebase. Also see Squashing commits with rebase.

- Tests Add relevant tests and make sure all existing ones still passes.

- No Warnings Make sure your code do not produce any build warnings.

- Make sure your commits follow the angular [Commit message guidelines](https://github.com/angular/angular/blob/master/CONTRIBUTING.md#commit)
