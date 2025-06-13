param (
	[switch]$NoArchive,
	[string]$ModName,
	[string]$OutputDirectory = $PSScriptRoot
)

$MainDir = "$PSScriptRoot" + "/" + $ModName
if (-not (Test-Path $MainDir))
{
	Write-Error -Message "Could not find subdirectory, is mod name set?: $MainDir" -ErrorAction Stop
}

Set-Location "$MainDir"
$FilesToInclude = "info.json","build/*"

$modInfo = Get-Content -Raw -Path "info.json" | ConvertFrom-Json
$modId = $modInfo.Id
$modVersion = $modInfo.Version

$DistDir = "$OutputDirectory/dist"
if ($NoArchive) {
	$ZipWorkDir = "$OutputDirectory"
} else {
	$ZipWorkDir = "$DistDir/tmp"
}
$ZipOutDir = "$ZipWorkDir/$modId"

New-Item "$ZipOutDir" -ItemType Directory -Force
Copy-Item -Force -Path $FilesToInclude -Destination "$ZipOutDir" -Recurse -Container

if (!$NoArchive)
{
	$FILE_NAME = "$DistDir/${modId}_v$modVersion.zip"
	Compress-Archive -Update -CompressionLevel Fastest -Path "$ZipOutDir/*" -DestinationPath "$FILE_NAME"
}
