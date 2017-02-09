<<<<<<< HEAD
param($installPath, $toolsPath, $package, $project)

. (Join-Path $toolsPath common.ps1)

if ($scriptsFolderProjectItem -eq $null) {
    # No Scripts folder
    Write-Host "No Scripts folder found"
    exit
}

# Update the _references.js file
=======
param($installPath, $toolsPath, $package, $project)

. (Join-Path $toolsPath common.ps1)

if ($scriptsFolderProjectItem -eq $null) {
    # No Scripts folder
    Write-Host "No Scripts folder found"
    exit
}

# Update the _references.js file
>>>>>>> origin/master
AddOrUpdate-Reference $scriptsFolderProjectItem $modernizrFileNameRegEx $modernizrFileName