<#PSScriptInfo
.VERSION 0.0.0
.GUID 4e7d7c0c-923d-4cd6-b97d-173a687f3681
.AUTHOR Henrik Lau Eriksson
.COMPANYNAME
.COPYRIGHT
.TAGS PowerToys Run Plugins Release
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
    Generate release notes snippets for the plugins.

    .Description
    Gathers information about the plugins and generates a markdown file with release notes snippets.

    .Example
    .\release.ps1

    .Link
    https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins
#>

# Plugins
$folders = Get-ChildItem -Path .\src -Directory -Exclude "*UnitTests", "libs"

# Archives
$files = Get-ChildItem -Path .\src -File -Include "*.zip" -Recurse

# Version
[xml]$props = Get-Content -Path "Directory.Build.props"
$version = "$($props.Project.PropertyGroup.Version)".Trim()

# Platforms
$platforms = "$($props.Project.PropertyGroup.Platforms)".Trim() -split ";"

# Output
$result = "docs\release-$version.md"

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

foreach ($folder in $folders) {
    $name = $($folder.Name.Split(".")[-1])

    Write-Line "## $name"
    Write-Line ""
    Write-Line "| Platform | Filename | Downloads"
    Write-Line "| --- | --- | ---"
    foreach ($file in $files | Where-Object { $_ -match $name }) {
        $zip = $file.Name
        $platform = Get-Platform $zip
        $url = "https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins/releases/download/v$version/$zip"

        Write-Line "| ``$platform`` | [$zip]($url) | [![$zip](https://img.shields.io/github/downloads/hlaueriksson/Community.PowerToys.Run.Plugins/v$version/$zip)]($url)"
    }
    Write-Line ""
}

Write-Line "## Installer Hashes"
Write-Line ""
Write-Line "| Filename | SHA256 Hash"
Write-Line "| --- | ---"
foreach ($file in $files) {
    $zip = $file.Name
    $hash = Get-FileHash $file -Algorithm SHA256 | Select-Object -ExpandProperty Hash

    Write-Line "| ``$zip`` | ``$hash``"
}
