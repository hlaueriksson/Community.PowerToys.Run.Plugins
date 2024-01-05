dotnet build -c Release /p:TF_BUILD=true

xcopy /y .\Images\ .\bin\Release\Dice\Images\
xcopy /y .\plugin.json .\bin\Release\Dice\
xcopy /y .\bin\Release\net8.0-windows\Community.PowerToys.Run.Plugin.Dice.* .\bin\Release\Dice\

:: Zip '.\bin\Release\Dice\' into 'Community.PowerToys.Run.Plugin.Dice.0.0.0.zip'
