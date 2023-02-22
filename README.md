# CzechAresClient
[![NuGet version (GrafanaApiClient)](https://img.shields.io/nuget/v/CzechAresClient)](https://www.nuget.org/packages/CzechAresClient)


.NET client library for Czech ARES API https://wwwinfo.mfcr.cz/ares/ares_xml.html.cz

By now it supports only very trimmed basic GET search by CompanyId (IČO)

Look at example in Demo

## Getting started

   ```
   Package Manager : Install-Package CzechAresClient
   CLI : dotnet add package CzechAresClient
   ```
Add using and ICzechAresClientService as a service in Program.cs 

   ```csharp
   using CzechAresClient;
   ```

   ```csharp
   builder.Services.AddTransient<ICzechAresClientService, CzechAresClientService>();
   builder.Services.AddHttpClient(); // do not forget this, its used by the lib
   ```
## Using
Inject service then use service as following:

   ```csharp
   var aresResult = await _aresClient.SearchByCompanyIdAsync("06279996"); // Search by IČO
   ```
   
   
   
 # Contribution
 Feel free to contribute and extend
