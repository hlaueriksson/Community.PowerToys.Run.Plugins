<#PSScriptInfo
.VERSION 0.87.0
.GUID d790e6d3-96c9-447b-9863-941da73870ea
.AUTHOR Henrik Lau Eriksson
.COMPANYNAME
.COPYRIGHT
.TAGS PowerToys Run Plugins Release
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
    Generate release notes snippets for the plugins.

    .Description
    Gathers information about the plugins and generates a markdown file with release notes snippets.

    .Example
    .\releasenotes.ps1

    .Link
    https://github.com/hlaueriksson/Community.PowerToys.Run.Plugin.Templates
#>

function Write-Line {
    param (
        [string]$line
    )

    $line | Add-Content -Path $result
}

function Get-Platform {
    param (
        [string]$filename
    )

    if ($filename -Match $platforms[0]) {
        $platforms[0]
    }
    else {
        $platforms[1]
    }
}

# Output
$result = "docs\releasenotes.md"

Set-Content -Path $result -Value ""

# Platforms
$platforms = ([xml](Get-Content -Path "Directory.Build.props")).Project.PropertyGroup.Platforms -split ";" | Where-Object { $_ -ne "" }

# Plugins
$folders = Get-ChildItem -Recurse -Filter "plugin.json" | Where-Object { $_.FullName -notlike "*\bin\*" } | ForEach-Object { $_.Directory } | Sort-Object -Unique

# Packages
$packages = Get-ChildItem -Path .\src -File -Include "*.zip" -Recurse

Write-Output "Doc:"

foreach ($folder in $folders) {
    Write-Output "- $($folder.Name)"

    $name = $($folder.Name.Split(".")[-1])

    # Version, Website
    $json = Get-Content -Path (Join-Path $folder.FullName "plugin.json") -Raw | ConvertFrom-Json
    $version = $json.Version
    $website = $json.Website

    $files = Get-ChildItem -Path $folder -File -Include "$name-$version*.zip" -Recurse

    Write-Line "## $name"
    Write-Line ""
    Write-Line "| Platform | Filename | Downloads"
    Write-Line "| --- | --- | ---"
    foreach ($file in $files) {
        $zip = $file.Name
        $platform = Get-Platform $zip
        $url = "$website/releases/download/v$version/$zip"
        $badge = "https://img.shields.io/github/downloads/$($website.Replace('https://github.com/', ''))/v$version/$zip"

        Write-Line "| ``$platform`` | [$zip]($url) | [![$zip]($badge)]($url)"
    }
    Write-Line ""
}

Write-Line "## Installer Hashes"
Write-Line ""
Write-Line "| Filename | SHA256 Hash"
Write-Line "| --- | ---"
foreach ($package in $packages) {
    $zip = $package.Name
    $hash = Get-FileHash $package -Algorithm SHA256 | Select-Object -ExpandProperty Hash

    Write-Line "| ``$zip`` | ``$hash``"
}
