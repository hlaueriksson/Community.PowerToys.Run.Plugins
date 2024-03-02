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

    .Parameter Plugin
    Plugin to deploy

    .Example
    .\deploy.ps1

    .Example
    .\deploy.ps1 -plugin Bang

    .Link
    https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins
#>
param (
    [ValidateSet("ARM64", "x64")]
    [string]$platform = "x64",

    [string]$plugin
)

#Requires -RunAsAdministrator

Stop-Process -Name "PowerToys" -Force -ErrorAction SilentlyContinue

# Plugins
$folders = Get-ChildItem -Path .\src -Directory -Exclude "*UnitTests", "libs" | Where-Object { $_ -match $plugin }

# Build
if ($folders.Count -eq 1) {
    dotnet build $folders[0] -c Release /p:TF_BUILD=true /p:Platform=$platform
}
else {
    dotnet build -c Release /p:TF_BUILD=true /p:Platform=$platform
}

Write-Output "Platform: $platform"

Write-Output "Deploy:"
foreach ($folder in $folders) {
    Write-Output "- $($folder.Name)"

    $name = $($folder.Name.Split(".")[-1])

    Remove-Item -LiteralPath "$env:LOCALAPPDATA\Microsoft\PowerToys\PowerToys Run\Plugins\$name" -Recurse -Force -ErrorAction SilentlyContinue
    Copy-Item -Path "$folder\bin\$platform\Release\net8.0-windows" -Destination "$env:LOCALAPPDATA\Microsoft\PowerToys\PowerToys Run\Plugins\$name" -Recurse -Force
}

$machinePath = "C:\Program Files\PowerToys\PowerToys.exe"
$userPath = "$env:LOCALAPPDATA\PowerToys\PowerToys.exe"

if (Test-Path $machinePath) {
    Start-Process -FilePath $machinePath
}
elseif (Test-Path $userPath) {
    Start-Process -FilePath $userPath
}
else {
    Write-Error "Unable to start PowerToys"
}
