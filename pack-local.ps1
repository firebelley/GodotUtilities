#Requires -Version 5
# Packs Firebelley.GodotUtilities and Firebelley.GodotUtilities.SourceGenerators into ./local-feed
# so Alchemortis (or any consumer) can restore live local changes when its UseLocalGodotUtilities
# toggle is on. Each pack gets a fresh version (unix-seconds patch), so NuGet never serves stale
# bits from its global cache.
#
# Usage:  ./pack-local.ps1            (packs 1000.0.<unix-seconds>)
#
# The version is a deliberately high STABLE sentinel (1000.0.*) that:
#   - sorts ABOVE any published release, so a floating 1000.0.* reference resolves here, and
#   - is stable (no prerelease tag), so it satisfies stable dependency constraints such as the
#     runtime package's ">= 6.0.0" dependency on the generator (a prerelease would be excluded).
# This base must match the floating version Alchemortis references (1000.0.*).
# The real package <Version> (and the compiled assembly version) are unaffected by this.
param(
    [string]$VersionPrefix = "1000.0",
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"

$root = $PSScriptRoot
$feed = Join-Path $root "local-feed"
$runtimeProject = Join-Path $root "GodotUtilities/GodotUtilities.csproj"
$generatorProject = Join-Path $root "SourceGenerators/SourceGenerators.csproj"

New-Item -ItemType Directory -Force -Path $feed | Out-Null

# Only the newest pack is ever consumed (the floating reference picks the highest), so clear
# stale packs to keep the feed from growing without bound.
Get-ChildItem -Path $feed -Filter "*.nupkg" -ErrorAction SilentlyContinue | Remove-Item -Force

$version = "$VersionPrefix.$([DateTimeOffset]::UtcNow.ToUnixTimeSeconds())"

# Pack the generator first, then the runtime library. The generator is a separate package that a
# consumer must reference directly to override the published one pulled in transitively. The feed
# is added as a restore source so the runtime can resolve the just-packed local generator (the
# runtime's declared generator dependency may not be published yet).
#
# Build with --no-incremental (a full Rebuild) before packing: a normal incremental build does not
# reliably recompile when only the embedded .sbncs template changes, which would ship a stale
# generator. pack then runs --no-build to package the freshly built output.
foreach ($project in @($generatorProject, $runtimeProject))
{
    dotnet build $project -c $Configuration --no-incremental "-p:RestoreAdditionalProjectSources=$feed"
    if ($LASTEXITCODE -ne 0)
    {
        throw "dotnet build failed for $project with exit code $LASTEXITCODE"
    }

    dotnet pack $project -c $Configuration --no-build -p:PackageVersion=$version -o $feed
    if ($LASTEXITCODE -ne 0)
    {
        throw "dotnet pack failed for $project with exit code $LASTEXITCODE"
    }
}

# Drop the cached generator from the Roslyn build server so the next consumer build loads the
# freshly packed analyzer instead of the one held in memory.
dotnet build-server shutdown | Out-Null

Write-Host ""
Write-Host "Packed Firebelley.GodotUtilities + .SourceGenerators $version" -ForegroundColor Green
Write-Host "Feed: $feed"
