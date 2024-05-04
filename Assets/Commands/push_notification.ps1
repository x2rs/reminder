param($title,$text,$icon_path) # path must be ascii

Add-Type -AssemblyName System.Windows.Forms 

$global:balloon = New-Object System.Windows.Forms.NotifyIcon
$balloon.Icon = $icon_path 
$balloon.BalloonTipIcon = [System.Windows.Forms.ToolTipIcon]::Info 
$balloon.BalloonTipText = $text
$balloon.BalloonTipTitle = $title
$balloon.Visible = $true 
$balloon.ShowBalloonTip(10)