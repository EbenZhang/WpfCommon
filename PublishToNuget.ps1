[CmdletBinding()]
param (   
    [Parameter(Mandatory=$True)]
    [string]$apikey
)
$ErrorActionPreference = 'Stop'
$file = gci -recurse -filter "Nicologies.WpfCommon.*.nupkg" -File -Path dist
nuget push $file.FullName -Source "https://www.nuget.org/api/v2/package" -ApiKey $apikey
