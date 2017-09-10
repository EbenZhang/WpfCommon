$ErrorActionPreference = 'Stop'
Remove-Item -Force ./dist/*nupkg -ErrorAction Ignore
nuget pack WpfCommon.csproj -OutputDirectory dist -Prop Configuration=Release
