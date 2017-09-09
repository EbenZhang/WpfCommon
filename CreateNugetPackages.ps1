$ErrorActionPreference = 'Stop'
Remove-Item -Force ./dist/*nupkg -ErrorAction Ignore
nuget pack WpfCommon.nuspec -OutputDirectory dist
