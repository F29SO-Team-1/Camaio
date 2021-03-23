# Contributing to the project

### This document sets out the guidelines for contributing changes/new features to the project.

**Before contributing, make sure you have installed all the necessary dependencies, which are outlined in [GETTING_STARTED.md](GETTING_STARTED.md).**

Make sure the tests pass on Actions section located on github. Before working on any new features, make the current ones pass the test suite.

When changing the codebase, **never** do so on branch `main`, and commits straight to `develop` should be avoided.

Before starting **any work**, make sure your local repository is up to date with the GitHub repository using `git pull`.

To start work on fixing and issue or implementing a new feature, create a new branch off of `develop`.

First, make sure you are on branch `develop`, run `git checkout develop`.

Then create your new branch:
- You can do so by running `git branch <new-branch>`, which will create a new branch from the one you are currently on, and then changing to that branch with `git checkout <new-branch>`.
- You can also use `git checkout -b <new-branch>` which will create a new branch from your current branch and change you to it.

If you want to contribute to a branch that is already on this github repo:
- Run `git pull`
- Run `git checkout origin/<branch-name>`
- Run `git checkout -b <branch-name>`

Then proceed to make your changes and `git commit` your work when appropriate.
Please follow best [practices](http://chris.beams.io/posts/git-commit/) when writing commit messages.

When you want to push your new branch up to the GitHub repository you can run `git push`, the first time you do this you will be prompted to run `git push --set-upstream origin <new-branch>`.

Do not use the `-f` or `--force` flag when using `git push` unless you are **sure** of what you are doing.

Once the branch has been successfully pushed to GitHub, open a pull-request, and the rest of the team can view and review your proposed changes.

Ideally you should push branch to the repo **as soon as you can**, so that everyone can see what everyone else is working on.