# Getting Started

This document outlines some ways for installng this project dependencies.

## Installing Dependencies

### C#

##### MacOS / Linux
If you are going to use the editor [vscode](https://code.visualstudio.com/download), you can download the following [extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp/).

But if you are going to use [visual studio](https://visualstudio.microsoft.com/downloads/) I don't think you are going to need anything.

#### Windows
If you are going to use the editor [vscode](https://code.visualstudio.com/download), you can download the following [extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp/).

But if you are going to use [visual studio](https://visualstudio.microsoft.com/downloads/) I don't think you are going to need anything.

### .Net Core

##### MacOS / Linux
All you need is the [SDK](https://dotnet.microsoft.com/download).

#### Windows
You have to download the SDK and the run time, which can be found [here](https://dotnet.microsoft.com/download)

## Getting the project running
Once all the dependencies are installed, you can start getting the project running.

First of all clone the git repository onto your local computer.

`git clone git@github.com:F29SO-Team-1/Camaio.git`

or

`git clone https://github.com/F29SO-Team-1/Camaio.git`

Then, navigate to the folder where the project was cloned.

`cd Camaio`

have the dependencies downloaded and change branch from the main to feature, the following command is to change branch.

`git checkout feature`

to check which branch you are on, use the following command

`git branch`

or

`git status`

Once on branch, feature. The following command will start the application.

`dotnet watch run`

Browse to https://localhost:5001, refresh the page, and verify the changes are displayed.

