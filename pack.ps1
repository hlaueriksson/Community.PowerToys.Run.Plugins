<#PSScriptInfo
.VERSION 0.0.0
.GUID f0754210-fa11-49ae-a3f6-7a06f0909b54
.AUTHOR Henrik Lau Eriksson
.COMPANYNAME
.COPYRIGHT
.TAGS PowerToys Run Plugins Pack
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
    Packs the plugins into release archives.

    .Description
    Builds the solution in Release configuration,
    copies the output files into plugin folders,
    packs the plugin folders into release archives.

    .Example
    .\pack.ps1

    .Link
    https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins
#>

# Clean
Get-ChildItem -Path ".\src" -Include "bin", "obj" -Recurse | Remove-Item -Recurse -Force

# Build
dotnet build $path -c Release /p:TF_BUILD=true

if (!$?) {
    # Build FAILED.
    Exit $LastExitCode
}

# Version
[xml]$props = Get-Content -Path "Directory.Build.props"
$version = $props.Project.PropertyGroup.Version
Write-Output "Version: $version"

# Plugins
$folders = Get-ChildItem -Path .\src -Directory -Exclude "*UnitTests", "libs"
$libs = Get-ChildItem -Path .\src\libs
$output = "\bin\x64\Release\net8.0-windows\"

Write-Output "Pack:"
foreach ($folder in $folders) {
    Write-Output "- $($folder.Name)"
    Copy-Item -Path "$($folder)$($output)" -Destination "$folder\bin\$($folder.Name)" -Recurse -Exclude $libs
    Compress-Archive -Path "$folder\bin\$($folder.Name)" -DestinationPath "$folder\bin\$($folder.Name).$version.zip"
}
