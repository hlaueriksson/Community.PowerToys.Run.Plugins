call pack.bat

taskkill /f /im PowerToys.exe /t

xcopy /s /y .\bin\Release\Dice\ "%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins\Dice\"

start "" "C:\Program Files\PowerToys\PowerToys.exe"
