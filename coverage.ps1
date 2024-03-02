# Clean
Get-ChildItem -Path ".\src" -Directory -Include "bin", "obj", "TestResults" -Recurse | Remove-Item -Recurse -Force
Remove-Item -LiteralPath "TestResults" -Recurse -Force

# Test
dotnet test --collect "Code Coverage" --settings coverage.runsettings /p:Platform=x64

# Report
reportgenerator -reports:"src\**\coverage.xml" -targetdir:"TestResults\Coverage" -reporttypes:Html -filefilters:"-*RegexGenerator.g.cs;-*Moq*;-*mockhttp*;-*Microsoft.Testing.Platform*"
