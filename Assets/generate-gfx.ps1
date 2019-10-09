$aseprite="Aseprite.exe"
$gfxDir="..\Source\Game\res"

function ResizeImage {
	param($file)
    $directory = [System.IO.Path]::GetDirectoryName($file)
    $directory = Join-Path $gfxDir $directory
    $fileName = [io.path]::GetFileNameWithoutExtension($file)
    $fileName = "$fileName.png"

	$targetFile = Join-Path $directory $fileName

    If(!(test-path $directory))
    {
        New-Item -ItemType Directory -Force -Path $directory | Out-Null
    }

    Write-Output " Processing $file"
	Invoke-Expression "$aseprite -b $file --scale 3 --save-as $targetFile"
}

$files = Get-ChildItem $PSScriptRoot -Filter *.aseprite -Recurse
$files | Foreach-Object {
    $imageFile = $_ | Resolve-Path -Relative

    ResizeImage($imageFile)
}

Write-Host "All done!"