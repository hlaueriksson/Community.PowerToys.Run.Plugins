call pack.bat

taskkill /f /im PowerToys.exe /t

xcopy /s /y .\bin\Release\DenCode\ "C:\Program Files\PowerToys\modules\launcher\Plugins\DenCode\"

start "" "C:\Program Files\PowerToys\PowerToys.exe"
