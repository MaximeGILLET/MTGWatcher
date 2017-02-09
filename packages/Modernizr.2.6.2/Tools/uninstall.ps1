<<<<<<< HEAD
param($installPath, $toolsPath, $package, $project)

. (Join-Path $toolsPath common.ps1)

# Update the _references.js file
=======
param($installPath, $toolsPath, $package, $project)

. (Join-Path $toolsPath common.ps1)

# Update the _references.js file
>>>>>>> origin/master
Remove-Reference $scriptsFolderProjectItem $modernizrFileNameRegEx