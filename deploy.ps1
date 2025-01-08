<#PSScriptInfo
.VERSION 0.87.0
.GUID 2d1e62b4-4b98-4fad-98b2-2cc1db4694b8
.AUTHOR Henrik Lau Eriksson
.COMPANYNAME
.COPYRIGHT
.TAGS PowerToys Run Plugins Deploy
.LICENSEURI
.PROJECTURI https://github.com/hlaueriksson/Community.PowerToys.Run.Plugin.Templates
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
    https://github.com/hlaueriksson/Community.PowerToys.Run.Plugin.Templates
#>
param (
    [ValidateSet("ARM64", "x64")]
    [string]$platform = "x64",

    [string]$plugin
)

#Requires -RunAsAdministrator

Stop-Process -Name "PowerToys" -Force -ErrorAction SilentlyContinue

# Plugins
$folders = Get-ChildItem -Recurse -Filter "plugin.json" | Where-Object { $_.FullName -notlike "*\bin\*" } | ForEach-Object { $_.Directory } | Sort-Object -Unique | Where-Object { $_ -match $plugin }

# Build
if ($folders.Count -eq 1) {
    dotnet build $folders[0] -c Release /p:TF_BUILD=true /p:Platform=$platform
}
else {
    dotnet build -c Release /p:TF_BUILD=true /p:Platform=$platform
}

$dependencies = @("PowerToys.Common.UI.*", "PowerToys.ManagedCommon.*", "PowerToys.Settings.UI.Lib.*", "Wox.Infrastructure.*", "Wox.Plugin.*")

# TargetFramework
$targetFramework = ([xml](Get-Content -Path "Plugin.props")).Project.PropertyGroup.TargetFramework

Write-Output "Platform: $platform"

Write-Output "Deploy:"
foreach ($folder in $folders) {
    Write-Output "- $($folder.Name)"

    $name = $($folder.Name.Split(".")[-1])

    Remove-Item -LiteralPath "$env:LOCALAPPDATA\Microsoft\PowerToys\PowerToys Run\Plugins\$name" -Recurse -Force -ErrorAction SilentlyContinue
    Copy-Item -Path "$folder\bin\$platform\Release\$targetFramework" -Destination "$env:LOCALAPPDATA\Microsoft\PowerToys\PowerToys Run\Plugins\$name" -Recurse -Force -Exclude $dependencies
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
