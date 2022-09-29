dotnet build -c Release /p:TF_BUILD=true

xcopy /y .\Images\ .\bin\Release\Bang\Images\
xcopy /y .\plugin.json .\bin\Release\Bang\
xcopy /y .\bin\Release\net6.0-windows\Community.PowerToys.Run.Plugin.Bang.* .\bin\Release\Bang\

:: Zip '.\bin\Release\Bang\' into 'Community.PowerToys.Run.Plugin.Bang.0.0.0.zip'
