dotnet restore
dotnet publish ./IO.Processor.Client.Learning/IO.Processor.Client.Learning.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o ./builds
dotnet publish ./IO.Processor.Server.Learning/IO.Processor.Server.Learning.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o ./builds


dotnet publish ./AnonymousPipeClient/AnonymousPipeClient.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o ./builds