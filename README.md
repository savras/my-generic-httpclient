# my-generic-httpclient
Generic HttpClient to make Web API calls on the server.

##Usage
Reference both projects in your solution (e.g. WebApi project).

In the IoC container registration (Autofac in this example), do the following:
```csharp
builder.Register(ctx =>
{
    // Add the appropriate values to app.config.
    // I prefer to split out the passwords to a pasword.config using the "file" attribute.
    var username = ConfigurationManager.AppSettings[HttpClientConstants.MySiteUsernameKey] ?? string.Empty;
    var password = ConfigurationManager.AppSettings[HttpClientConstants.MySitePasswordKey] ?? string.Empty;
    var uri = HttpClientConstants.MySiteUri;
    return new MySiteHttpClient(uri, username, password);
}).As<IMySiteHttpClient>().SingleInstance();  
// MUST be single instance otherwise it will chew up the sockets: 
// https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
```