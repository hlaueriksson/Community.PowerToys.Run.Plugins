dotnet build -c Release /p:TF_BUILD=true

xcopy /y .\Images\ .\bin\Release\Twitch\Images\
xcopy /y .\plugin.json .\bin\Release\Twitch\
xcopy /y .\bin\Release\net8.0-windows\Community.PowerToys.Run.Plugin.Twitch.* .\bin\Release\Twitch\

:: Zip '.\bin\Release\Twitch\' into 'Community.PowerToys.Run.Plugin.Twitch.0.0.0.zip'
