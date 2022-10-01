call pack.bat

taskkill /f /im PowerToys.exe /t

xcopy /s /y .\bin\Release\Need\ "C:\Program Files\PowerToys\modules\launcher\Plugins\Need\"

start "" "C:\Program Files\PowerToys\PowerToys.exe"
