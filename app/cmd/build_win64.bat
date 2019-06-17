
cd app
dotnet publish -c Release -r win-x64 -o Publish\\Win64
REM xcopy ../Template/*.* bin\\Release\\netcoreapp2.1\\win-x64
REM echo %cd%
REM pause