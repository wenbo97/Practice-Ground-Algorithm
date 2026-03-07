dotnet restore
dotnet publish ./Automation.Build.Plugin/Automation.Build.Plugin.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o ./builds