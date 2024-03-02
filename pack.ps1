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
Get-ChildItem -Path ".\src" -Directory -Include "bin", "obj" -Recurse | Remove-Item -Recurse -Force

# Version
[xml]$props = Get-Content -Path "Directory.Build.props"
$version = "$($props.Project.PropertyGroup.Version)".Trim()
Write-Output "Version: $version"

# Platforms
$platforms = "$($props.Project.PropertyGroup.Platforms)".Trim() -split ";"

# Plugins
$folders = Get-ChildItem -Path .\src -Directory -Exclude "*UnitTests", "libs"
$libs = Get-ChildItem -Path .\src\libs -File -Recurse

foreach ($platform in $platforms)
{
    Write-Output "Platform: $platform"

    # Build
    dotnet build -c Release /p:TF_BUILD=true /p:Platform=$platform

    if (!$?) {
        # Build FAILED.
        Exit $LastExitCode
    }

    Write-Output "Pack:"
    foreach ($folder in $folders) {
        Write-Output "- $($folder.Name)"

        $name = $($folder.Name.Split(".")[-1])
        $output = "$folder\bin\$platform\Release\net8.0-windows\"
        $destination = "$folder\bin\$platform\$name"
        $zip = "$folder\bin\$platform\$name-$version-$($platform.ToLower()).zip"

        Copy-Item -Path $output -Destination $destination -Recurse -Exclude $libs
        Compress-Archive -Path $destination -DestinationPath $zip
    }
}
