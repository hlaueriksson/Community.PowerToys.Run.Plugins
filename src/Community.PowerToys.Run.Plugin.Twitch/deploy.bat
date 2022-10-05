call pack.bat

taskkill /f /im PowerToys.exe /t

xcopy /s /y .\bin\Release\Twitch\ "C:\Program Files\PowerToys\modules\launcher\Plugins\Twitch\"

start "" "C:\Program Files\PowerToys\PowerToys.exe"
