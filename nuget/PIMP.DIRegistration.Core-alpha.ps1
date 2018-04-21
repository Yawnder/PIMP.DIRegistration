. .\API_KEY.ps1
Remove-Item -path packages\*
nuget.exe pack ..\src\PIMP.DIRegistration.Core\PIMP.DIRegistration.Core.csproj -Symbols -IncludeReferencedProjects -OutputDirectory packages -Suffix alpha
nuget push packages\PIMP.DIRegistration.Core.*.symbols.nupkg -Source https://api.nuget.org/v3/index.json -ApiKey $apiKey