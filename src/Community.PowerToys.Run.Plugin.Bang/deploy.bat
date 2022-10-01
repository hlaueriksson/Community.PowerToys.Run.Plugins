call pack.bat

taskkill /f /im PowerToys.exe /t

xcopy /s /y .\bin\Release\Bang\ "C:\Program Files\PowerToys\modules\launcher\Plugins\Bang\"

start "" "C:\Program Files\PowerToys\PowerToys.exe"
