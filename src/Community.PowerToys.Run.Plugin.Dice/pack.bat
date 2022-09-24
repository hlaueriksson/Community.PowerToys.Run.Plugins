dotnet build -c Release /p:TF_BUILD=true

xcopy /y .\Images\ .\bin\Release\Dice\Images\
xcopy /y .\appsettings.json .\bin\Release\Dice\
xcopy /y .\plugin.json .\bin\Release\Dice\
xcopy /y .\bin\Release\net6.0-windows\Community.PowerToys.Run.Plugin.Dice.* .\bin\Release\Dice\

:: Zip '.\bin\Release\Dice\' into 'Community.PowerToys.Run.Plugin.Dice.0.0.0.zip'
