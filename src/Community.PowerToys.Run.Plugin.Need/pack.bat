dotnet build -c Release /p:TF_BUILD=true

xcopy /y .\Images\ .\bin\Release\Need\Images\
xcopy /y .\plugin.json .\bin\Release\Need\
xcopy /y .\bin\Release\net7.0-windows\Community.PowerToys.Run.Plugin.Need.* .\bin\Release\Need\

:: Zip '.\bin\Release\Need\' into 'Community.PowerToys.Run.Plugin.Need.0.0.0.zip'
