call pack.bat

taskkill /f /im PowerToys.exe /t

xcopy /s /y .\bin\Release\Dice\ "C:\Program Files\PowerToys\modules\launcher\Plugins\Dice\"

start "" "C:\Program Files\PowerToys\PowerToys.exe"
