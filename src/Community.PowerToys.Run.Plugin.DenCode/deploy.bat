call pack.bat

taskkill /f /im PowerToys.exe /t

xcopy /s /y .\bin\Release\DenCode\ "%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins\DenCode\"

start "" "C:\Program Files\PowerToys\PowerToys.exe"
