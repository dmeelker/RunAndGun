$aseprite="Aseprite.exe"
$gfxDir="..\Source\Game\Resources"

function ResizeImage {
	param($file)
	$scale = 3
    $directory = [System.IO.Path]::GetDirectoryName($file)
Write-Output $directory
    if($directory -eq ".\Backdrops")
    {
    	$scale = 2
    }

    $directory = Join-Path $gfxDir $directory
    $fileName = [io.path]::GetFileNameWithoutExtension($file)
    $fileName = "$fileName.png"

	$targetFile = Join-Path $directory $fileName

    If(!(test-path $directory))
    {
        New-Item -ItemType Directory -Force -Path $directory | Out-Null
    }

    Write-Output " Processing $file"
	Invoke-Expression "$aseprite -b $file --scale $scale --save-as $targetFile"
}

$files = Get-ChildItem $PSScriptRoot -Filter *.aseprite -Recurse
$files | Foreach-Object {
    $imageFile = $_ | Resolve-Path -Relative

    ResizeImage($imageFile)
}

Write-Host "All done!"