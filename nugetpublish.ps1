param(
     [Parameter()]
     [string]$NugetVersion
 )

dotnet nuget push ("./turtlecore/bin/debug/Woopec.Core." + $NugetVersion + ".nupkg") --api-key $Env:NUGETKEY --source https://api.nuget.org/v3/index.json
dotnet nuget push ("./turtlewpf/bin/debug/Woopec.Wpf." + $NugetVersion + ".nupkg") --api-key $Env:NUGETKEY --source https://api.nuget.org/v3/index.json
