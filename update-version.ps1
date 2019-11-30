if ($env:APPVEYOR_REPO_BRANCH -eq "master")
{
	write-host "It is master branch, update the version as a actual release"
	$ver = "$($env:APPVEYOR_BUILD_VERSION)"
	$verForRelease = $ver.SubString(0,$ver.lastIndexOf("-"))
	$parts = $verForRelease.Split(".")
	$major = $parts[0]
	$minor = $parts[1]
	$patch = $parts[2]
	$verForRelease = $major + "." + $minor + "." + $patch
	write-host "The version is now $verForRelease"
    Update-AppveyorBuild -Version "$verForRelease"
}
else
{
	write-host "non master branch"
    Update-AppveyorBuild -Version "$($env:APPVEYOR_BUILD_VERSION)"
}
$configFiles = Get-ChildItem . *.csproj -rec
$versionString = "<PackageVersion>" + $env:APPVEYOR_BUILD_VERSION + "</PackageVersion>"
foreach ($file in $configFiles)
{
    (Get-Content $file.PSPath) |
    Foreach-Object { $_ -replace "<PackageVersion>1.0.0</PackageVersion>", $versionString  } |
    Set-Content $file.PSPath
}
