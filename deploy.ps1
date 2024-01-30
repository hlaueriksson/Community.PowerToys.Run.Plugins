<#PSScriptInfo
.VERSION 0.0.0
.GUID c530171b-4918-4995-b5c2-7d2540b01152
.AUTHOR Henrik Lau Eriksson
.COMPANYNAME
.COPYRIGHT
.TAGS PowerToys Run Plugins Deploy
.LICENSEURI
.PROJECTURI https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins
.ICONURI
.EXTERNALMODULEDEPENDENCIES
.REQUIREDSCRIPTS
.EXTERNALSCRIPTDEPENDENCIES
.RELEASENOTES
#>

<#
    .Synopsis
    Deploys the plugins to PowerToys Run.

    .Description
    Copies the plugins to the PowerToys Run Plugins folder:
    - %LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins

    .Parameter Platform
    Platform: ARM64 | x64

    .Example
    .\deploy.ps1

    .Link
    https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins
#>
param (
    [ValidateSet("ARM64", "x64")]
    [string]$platform = "x64"
)

# Pack
Invoke-Expression -Command $PSScriptRoot\pack.ps1

Stop-Process -Name "PowerToys" -Force -ErrorAction SilentlyContinue

# Wait
Start-Sleep -Seconds 2

# Plugins
$folders = Get-ChildItem -Path .\src -Directory -Exclude "*UnitTests", "libs"

Write-Output "Platform: $platform"

Write-Output "Deploy:"
foreach ($folder in $folders) {
    Write-Output "- $($folder.Name)"

    $name = $($folder.Name.Split(".")[-1])

    Remove-Item -LiteralPath "$env:LOCALAPPDATA\Microsoft\PowerToys\PowerToys Run\Plugins\$name" -Recurse -Force -ErrorAction SilentlyContinue
    Copy-Item -Path "$folder\bin\$platform\$name" -Destination "$env:LOCALAPPDATA\Microsoft\PowerToys\PowerToys Run\Plugins\$name" -Recurse -Force
}

Start-Process -FilePath "C:\Program Files\PowerToys\PowerToys.exe"
