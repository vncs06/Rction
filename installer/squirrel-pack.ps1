param(
    [string]$Configuration = "Release",
    [string]$Framework = "net8.0-windows10.0.19041.0",
    [string]$Version = "",
    [string]$PackId = "JellyfinMauiClient",
    [string]$RemoteReleases = "" # e.g., https://github.com/OWNER/REPO/releases/download/v1.0.0
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$Root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent
$Project = Join-Path $Root 'JellyfinMauiClient/JellyfinMauiClient.csproj'

Write-Host "==> Publishing MAUI app ($Configuration, $Framework)"
dotnet publish $Project -c $Configuration -f $Framework -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=true -p:PublishSingleFile=false | Out-Host

$PublishDir = Join-Path $Root "JellyfinMauiClient/bin/$Configuration/$Framework/win10-x64/publish"
if (!(Test-Path $PublishDir)) { throw "Publish output not found: $PublishDir" }

if ([string]::IsNullOrWhiteSpace($Version)) {
    $csprojXml = [xml](Get-Content -LiteralPath $Project)
    $verNode = $csprojXml.Project.PropertyGroup | Where-Object { $_.ApplicationVersion } | Select-Object -First 1
    $Version = if ($verNode) { $verNode.ApplicationVersion } else { "1.0.0" }
}

$localTool = Join-Path $Root 'installer/tools/squirrel.exe'
$Squirrel = $null
if (Test-Path $localTool) {
    $Squirrel = $localTool
    Write-Host "==> Using local tools\\squirrel.exe"
} else {
    $Tool = "squirrel"
    Write-Host "==> Ensuring Clowd.Squirrel tool is installed"
    try {
        $null = & $Tool --help 2>$null
    } catch {
        dotnet tool install --global Clowd.Squirrel | Out-Host
    }
    $Squirrel = "${env:USERPROFILE}\.dotnet\tools\squirrel.exe"
}

if (!(Test-Path $Squirrel)) { throw "squirrel.exe not found. Place it at installer\\tools\\squirrel.exe ou instale via 'dotnet tool install --global Clowd.Squirrel'" }

$ReleaseDir = Join-Path $Root 'installer/dist'
New-Item -ItemType Directory -Force -Path $ReleaseDir | Out-Null

Write-Host "==> Packing with Squirrel ($PackId $Version)"
& $Squirrel pack --packId $PackId --packVersion $Version --packDirectory $PublishDir --releaseDir $ReleaseDir | Out-Host

if (-not [string]::IsNullOrWhiteSpace($RemoteReleases)) {
    Write-Host "==> Generating bootstrapper pointing to remote releases: $RemoteReleases"
    # Clowd.Squirrel pack embeds a bootstrapper Setup.exe that supports /update; to point to remote feed, generate a web-setup
    # This uses '--bootstrapper' to override the URL if supported. If not, provide a helper command for Update.exe
    try {
        & $Squirrel pack --packId $PackId --packVersion $Version --packDirectory $PublishDir --releaseDir $ReleaseDir --bootstrapper | Out-Host
    } catch {
        Write-Warning "Bootstrapper generation may depend on Squirrel version. As alternativa, distribua 'RELEASES' remoto e use Update.exe --update $RemoteReleases"
    }
}

Write-Host "==> Done. Files in $ReleaseDir"
