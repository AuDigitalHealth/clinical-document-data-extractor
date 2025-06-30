del *.nupkg

dotnet build /p:Configuration=Release

REM Use dotnet for packaging now
REM NuGet.exe pack CdaExtractor/CdaExtractor.csproj -Properties Configuration=Release
dotnet pack .\CdaExtractor\CdaExtractor.csproj -c Release -o .

pause

forfiles /m *.nupkg /c "cmd /c NuGet.exe push @FILE -Source https://www.nuget.org/api/v2/package"
