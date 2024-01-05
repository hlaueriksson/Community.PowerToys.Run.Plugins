call pack.bat

taskkill /f /im PowerToys.exe /t

xcopy /s /y .\bin\Release\Need\ "%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins\Need\"

start "" "C:\Program Files\PowerToys\PowerToys.exe"
