
KeePass password service.
[[_TOC_]]

## Installation
To setup this service as a nuget package you need to download a '[nuget.config](nuget.config)' file from this repo and put it in your solution main folder.
This file contains all data necessary for visual studio / nuget / dotnet commands to download nuget version of this project.

Next step is to donload nuget package like any other in visual studio / command line tool, using provided repository data.
In Visual Studio: 
- Right-click on chosen project 'dependencies' and select 'manage nuget packages'.
- In top-right corner select 'HSF_KeePass_Nuget_source from 'Package source:' dropdown.
- In the left window a 'KeePassPleasantPasswordServerClient' should show up - if not, try to hit refresh.
- Select it and hit 'Install'

## Updates
This projects nuget package can be upgraded like any other nuget, from nuget manager.

## Prerequisites

A loaded json file injected into 'IConfiguraion' containing following data:
>>>
"KeePass": {
    "BaseAddress": "correct address to KeePass instannce ended with '/'",
    "TokenEndpoint": "OAuth2/Token",
    "RestEndpoint": "api/v4/rest/credential/",
    "Username": "KeePass username",
    "Password": "KeePass password"
  }
>>>

If there is no loaded json file - those settings can be injected when setting up a Dependency Injection.

## Dependency injection setup
Example uses Microsoft NET 5.0 DI container.

This project contains simplified DI installation method called `SetupKeePassServices()`, that takes `IConfiguration` as a parameter.
Optionally, if you wish to replace / override settings from json file mentioned earlier, you can give it additional optional parameter `KeePassSettings`.
Settings from this object will supersede those from json.
This is the whole setup.

## Usage
Inject `IKeePassService` into any object you want.
Use async `AskForSecret(string guid)` method to retrieve a secret containg username / password corresponding to given guid.


