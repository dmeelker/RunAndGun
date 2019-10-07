param($targetDirectory)

Get-ChildItem $PSScriptRoot -Filter *.dll | Foreach-Object {
    $imageFile = $_ | Resolve-Path -Relative
    Write-Output "Copying $_"
    Copy-Item -Path $_.FullName -Destination $targetDirectory
}
