
dotnet restore DeepSleep.sln
nuget restore DeepSleep.sln

& "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe" DeepSleep.sln /t:Clean,Build /p:Configuration=Release
 
nuget pack DeepSleep\.pack\.nuget.local\DeepSleep.nuspec -Verbosity detailed -OutputDirectory ..\.nuget.local
nuget pack DeepSleep.NetCore\.pack\.nuget.local\DeepSleep.NetCore.nuspec -Verbosity detailed -OutputDirectory ..\.nuget.local
nuget pack DeepSleep.Validation.DataAnnotations\.pack\.nuget.local\DeepSleep.Validation.DataAnnotations.nuspec -Verbosity detailed -OutputDirectory ..\.nuget.local