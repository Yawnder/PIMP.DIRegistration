. .\API_KEY.ps1
nuget.exe pack ..\src\PIMP.DIRegistration.Core\PIMP.DIRegistration.Core.csproj -Symbols -OutputDirectory packages
nuget.exe pack ..\src\PIMP.DIRegistration.UnityContainer\PIMP.DIRegistration.UnityContainer.csproj -Symbols -OutputDirectory packages
nuget push packages\*.symbols.nupkg -Source https://api.nuget.org/v3/index.json -ApiKey $apiKey