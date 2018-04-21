. .\API_KEY.ps1
Remove-Item -path packages\*
nuget.exe pack ..\src\PIMP.DIRegistration.UnityContainer\PIMP.DIRegistration.UnityContainer.csproj -Symbols -IncludeReferencedProjects -OutputDirectory packages -Suffix alpha
nuget push packages\PIMP.DIRegistration.UnityContainer.*.symbols.nupkg -Source https://api.nuget.org/v3/index.json -ApiKey $apiKey