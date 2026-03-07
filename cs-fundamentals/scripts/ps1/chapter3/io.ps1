$contentFilePath = "$PSScriptRoot\demo.log"

$content = Get-Content -Path $contentFilePath

Write-Host $content

Get-Content $contentFilePath  -Head 1

Get-Content $contentFilePath -Wait -Tail 10

