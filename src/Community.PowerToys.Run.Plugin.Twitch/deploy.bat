call pack.bat

taskkill /f /im PowerToys.exe /t

xcopy /s /y .\bin\Release\Twitch\ "%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins\Twitch\"

start "" "C:\Program Files\PowerToys\PowerToys.exe"
