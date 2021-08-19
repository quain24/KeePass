
KeePass password service.
[[_TOC_]]

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


