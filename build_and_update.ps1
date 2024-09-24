$zipfile = "plugin.zip"
$woxfile = "custom_weather.wox"
$releasepath = "bin\Release\*"
$pluginpath = "$env:APPDATA\Wox\Plugins\"
$woxpath = "$env:LOCALAPPDATA\Wox\Wox.exe"

Set-Location $PSScriptRoot

#delete .zip and .wox file if exists
if (Test-Path $zipfile) {
	Remove-Item $zipfile
}
if (Test-Path $woxfile) {
	Remove-Item $woxfile
}

#create new .wox file
Compress-Archive $releasepath $zipfile
Rename-Item $zipfile $woxfile

#stop wox if running
$woxprocess = Get-Process -Name "Wox" -ErrorAction SilentlyContinue
if ($woxprocess) {
	$woxprocess.CloseMainWindow() | Out-Null
	Sleep 1
	if (!$woxprocess.started) {
		$woxprocess.Kill()
		Sleep 1
	}
	$woxprocess.Close()
}

#copy new releasepath files into plugin folder if exists
$foundplugin = $false
$plugins = Get-ChildItem $pluginpath
foreach ($plugin in $plugins) {
	if ($plugin.Name -like "Custom.Weather*") {
		$foundplugin = $true
		Copy-Item $releasepath $plugin.FullName -Recurse -Force
		break
	}
}
if (!$foundplugin) {
	echo "Custom Weather directory not found. Please install the plugin manually first."
}

#restart wox
Start-Process $woxpath
if ($foundplugin) {
	echo "Plugin successfully updated."
}
