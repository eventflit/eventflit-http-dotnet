
"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" .\eventflit-dotnet-everything-server.sln /t:Clean,Rebuild /p:Configuration=Release /fileLogger

tools\nuget.exe update -self
tools\nuget.exe pack .\EventflitServer\EventflitServer.csproj -verbosity detailed -properties Configuration=Release