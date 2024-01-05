call pack.bat

taskkill /f /im PowerToys.exe /t

xcopy /s /y .\bin\Release\Bang\ "%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins\Bang\"

start "" "C:\Program Files\PowerToys\PowerToys.exe"
