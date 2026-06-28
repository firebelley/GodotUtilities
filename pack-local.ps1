#Requires -Version 5
# Packs Firebelley.GodotUtilities into ./local-feed with a unique prerelease version so that
# Alchemortis (or any consumer) can restore live local changes when its UseLocalGodotUtilities
# toggle is on. Each pack gets a fresh timestamped version, so NuGet never serves stale bits
# from its global cache.
#
# Usage:  ./pack-local.ps1            (packs 1000.0.0-local.<timestamp>)
#
# The version base is a deliberately high sentinel (1000.0.0) so the local prerelease always
# sorts ABOVE any published stable release. A floating reference like 6.2.0-local.* would
# otherwise resolve to the published stable 6.2.0, because stable outranks a same-base prerelease.
# This base must match the floating version Alchemortis references (1000.0.0-local.*).
# The real package <Version> (and the compiled assembly version) are unaffected by this.
param(
    [string]$VersionPrefix = "1000.0.0",
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"

$root = $PSScriptRoot
$feed = Join-Path $root "local-feed"
$project = Join-Path $root "GodotUtilities/GodotUtilities.csproj"

New-Item -ItemType Directory -Force -Path $feed | Out-Null

# Only the newest pack is ever consumed (the floating reference picks the highest), so clear
# stale packs to keep the feed from growing without bound.
Get-ChildItem -Path $feed -Filter "*.nupkg" -ErrorAction SilentlyContinue | Remove-Item -Force

$version = "$VersionPrefix-local.$(Get-Date -Format 'yyyyMMddHHmmss')"

dotnet pack $project -c $Configuration -p:PackageVersion=$version -o $feed
if ($LASTEXITCODE -ne 0)
{
    throw "dotnet pack failed with exit code $LASTEXITCODE"
}

Write-Host ""
Write-Host "Packed Firebelley.GodotUtilities $version" -ForegroundColor Green
Write-Host "Feed: $feed"
