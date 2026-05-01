param(
    [string]$Serial,
    [switch]$KeepData,
    [switch]$NoLaunch
)

$ErrorActionPreference = "Stop"

$projectDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$gradleWrapper = Join-Path $projectDir "gradlew.bat"
$localPropertiesPath = Join-Path $projectDir "local.properties"
$appId = "com.nitkotin.android"
$activityName = ".MainActivity"
$apkPath = Join-Path $projectDir "app\build\outputs\apk\debug\app-debug.apk"

function Get-AndroidSdkDirectory {
    param([string]$LocalProperties)

    if (Test-Path $LocalProperties) {
        $sdkLine = Get-Content $LocalProperties | Where-Object { $_ -match '^sdk\.dir=' } | Select-Object -First 1
        if ($sdkLine) {
            $sdkValue = $sdkLine.Substring("sdk.dir=".Length)
            return $sdkValue -replace '\\\\', '\'
        }
    }

    if ($env:ANDROID_SDK_ROOT) {
        return $env:ANDROID_SDK_ROOT
    }

    if ($env:ANDROID_HOME) {
        return $env:ANDROID_HOME
    }

    throw "Android SDK directory was not found. Set ANDROID_SDK_ROOT or update local.properties."
}

function Invoke-Adb {
    param(
        [string]$AdbPath,
        [string]$DeviceSerial,
        [string[]]$Arguments
    )

    $fullArguments = @()
    if ($DeviceSerial) {
        $fullArguments += @("-s", $DeviceSerial)
    }
    $fullArguments += $Arguments

    & $AdbPath @fullArguments
    if ($LASTEXITCODE -ne 0) {
        throw "adb command failed: $($fullArguments -join ' ')"
    }
}

function Get-ConnectedDeviceSerial {
    param([string]$AdbPath)

    $output = & $AdbPath devices
    if ($LASTEXITCODE -ne 0) {
        throw "Unable to query adb devices."
    }

    $deviceLines = $output |
        Select-Object -Skip 1 |
        Where-Object { $_.Trim() } |
        Where-Object { $_ -notmatch '^(\*|adb server)' }

    $readyDevices = @()
    $blockedDevices = @()

    foreach ($line in $deviceLines) {
        $parts = $line -split "`t+|\s+", 2
        if ($parts.Count -lt 2) {
            continue
        }

        $serialNumber = $parts[0].Trim()
        $state = $parts[1].Trim()

        if ($state -eq "device") {
            $readyDevices += $serialNumber
        }
        else {
            $blockedDevices += "$serialNumber [$state]"
        }
    }

    if ($readyDevices.Count -eq 1) {
        return $readyDevices[0]
    }

    if ($readyDevices.Count -gt 1) {
        throw "Multiple Android devices are connected: $($readyDevices -join ', '). Re-run with -Serial <device-id>."
    }

    if ($blockedDevices.Count -gt 0) {
        throw "No authorized Android device is ready. Current adb states: $($blockedDevices -join ', '). Check the USB debugging prompt on the phone."
    }

    throw "No Android device detected. Connect the phone over USB and verify USB debugging is enabled."
}

if (-not (Test-Path $gradleWrapper)) {
    throw "Gradle wrapper not found at $gradleWrapper"
}

$sdkDirectory = Get-AndroidSdkDirectory -LocalProperties $localPropertiesPath
$adbPath = Join-Path $sdkDirectory "platform-tools\adb.exe"

if (-not (Test-Path $adbPath)) {
    throw "adb.exe was not found at $adbPath"
}

Write-Host "Using Android SDK: $sdkDirectory"
Write-Host "Building debug APK..."
& $gradleWrapper ":app:assembleDebug"
if ($LASTEXITCODE -ne 0) {
    throw "Gradle build failed."
}

if (-not (Test-Path $apkPath)) {
    throw "APK was not produced at $apkPath"
}

Invoke-Adb -AdbPath $adbPath -DeviceSerial $null -Arguments @("start-server")

if (-not $Serial) {
    $Serial = Get-ConnectedDeviceSerial -AdbPath $adbPath
}

Write-Host "Using device: $Serial"

if (-not $KeepData) {
    Write-Host "Removing previous app install if present..."
    & $adbPath -s $Serial uninstall $appId | Out-Host
}

Write-Host "Installing APK..."
Invoke-Adb -AdbPath $adbPath -DeviceSerial $Serial -Arguments @("install", "-r", "-t", $apkPath)

if (-not $NoLaunch) {
    Write-Host "Launching app..."
    Invoke-Adb -AdbPath $adbPath -DeviceSerial $Serial -Arguments @("shell", "am", "start", "-n", "$appId/$activityName")
}

Write-Host "Done."