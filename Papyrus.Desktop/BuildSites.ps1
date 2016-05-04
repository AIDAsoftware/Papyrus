$siteFiles = ls mkdocs.yml -recurse 
$originalPath = pwd

foreach($siteFile in $siteFiles) {
    cd $siteFile.Directory;
    Write-Host $siteFile.Directory;
    Invoke-Expression "& mkdocs build"
}

cd $originalPath