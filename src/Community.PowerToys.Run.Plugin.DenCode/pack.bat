dotnet build -c Release /p:TF_BUILD=true

xcopy /y .\Images\ .\bin\Release\DenCode\Images\
xcopy /y .\plugin.json .\bin\Release\DenCode\
xcopy /y .\bin\Release\net8.0-windows\Community.PowerToys.Run.Plugin.DenCode.* .\bin\Release\DenCode\

:: Zip '.\bin\Release\DenCode\' into 'Community.PowerToys.Run.Plugin.DenCode.0.0.0.zip'
