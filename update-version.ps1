$configFiles = Get-ChildItem . *.csproj -rec
$versionString = "<PackageVersion>" + $env:APPVEYOR_BUILD_VERSION + "</PackageVersion>"
foreach ($file in $configFiles)
{
    (Get-Content $file.PSPath) |
    Foreach-Object { $_ -replace "<PackageVersion>1.0.0</PackageVersion>", $versionString  } |
    Set-Content $file.PSPath
}
