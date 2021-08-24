## What is it?
It is a simple package enabling You to grab basic information from a KeePass server that You or Your organisation is hosting.
Data that You can get is a login and a password.

Ideal solution for having a centralised password storage for a single person or whole company.
Store Your passwords on a server and let Your services / apps ask for them when needed.


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

## No Dependency injection container setup
'New up' `KeePassService` and in place of `IHttpClientFactory` insert `NoDiHttpClientFactory` object - this will provide `KeePassService`
with proper `HttpClient` (singleton).

## Usage
Inject `IKeePassService` into any object you want.
Use async `AskForSecret(string guid)` method to retrieve a secret containg username / password corresponding to given guid.


