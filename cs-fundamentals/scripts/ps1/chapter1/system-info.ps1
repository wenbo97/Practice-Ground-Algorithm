Get-PSDrive

Get-ChildItem Env: | Select-Object -Property Name

Get-Date -Format "yyyy-MM-dd HH:mm:ss"


$watcher = [System.IO.FileSystemWatcher]::new("D:\A_Projects")

$watcher.EnableRaisingEvents = $true
$watcher.IncludeSubdirectories = $true

$subscription = Register-ObjectEvent -InputObject $watcher -EventName Created -Action {
    Write-Host "New file created: $($Event.SourceEventArgs.Name)"
}

try{
    Write-Host "Watching for changes... Press Ctrl+C to stop."
    while($true){
        Start-Sleep -Seconds 1
    }
}finally{
    Write-Host "Cleaning up..."
    Unregister-Event -SourceIdentifier $subscription.Name
    $watcher.Dispose()
    Write-Host "Done."
}