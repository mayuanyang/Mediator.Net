if ($env:APPVEYOR_REPO_TAG -eq "true" -And $env:APPVEYOR_REPO_BRANCH -eq "master")
{
    Update-AppveyorBuild -Version "$($env:APPVEYOR_REPO_TAG_NAME)"
}
else
{
    Update-AppveyorBuild -Version "$env:APPVEYOR_REPO_BRANCH-$($env:APPVEYOR_BUILD_VERSION)-ci$($env:APPVEYOR_BUILD_NUMBER)"
}
$configFiles = Get-ChildItem . *.csproj -rec
$versionString = "<PackageVersion>" + $env:APPVEYOR_BUILD_VERSION + "</PackageVersion>"
foreach ($file in $configFiles)
{
    (Get-Content $file.PSPath) |
    Foreach-Object { $_ -replace "<PackageVersion>1.0.0</PackageVersion>", $versionString  } |
    Set-Content $file.PSPath
}
