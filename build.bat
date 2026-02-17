@echo off

dotnet publish "MinecraftModTranslator\MinecraftModTranslator.csproj" -c Release -o publish/bin
dotnet publish "ExecuteShell\ExecuteShell.csproj" -c Release -r win-x64 --no-self-contained -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=false -o publish

pause