[CmdletBinding()]
param(
    [string]$Framework = "net10.0-android",
    [int]$ApiPort = 5080,
    [string]$ApiBindAddress = "0.0.0.0"
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$dotnetCommand = Get-Command dotnet -ErrorAction SilentlyContinue
$dotnet = if ($dotnetCommand) { $dotnetCommand.Source } else { "C:\Program Files\dotnet\dotnet.exe" }

if (-not (Test-Path $dotnet)) {
    throw "dotnet SDK introuvable. Installez le SDK .NET 10 et la workload MAUI."
}

$apiProject = Join-Path $root "backend\Mobilo.Api\Mobilo.Api.csproj"
$mauiProject = Join-Path $root "MobiloApp.csproj"
$api = $null

try {
    Write-Host "Starting Mobilo API on http://${ApiBindAddress}:$ApiPort ..." -ForegroundColor Cyan
    $api = Start-Process `
        -FilePath $dotnet `
        -ArgumentList @("run", "--project", "`"$apiProject`"", "--urls", "http://${ApiBindAddress}:$ApiPort") `
        -WorkingDirectory $root `
        -PassThru

    if ($Framework -like "net10.0-android*") {
        $adbCommand = Get-Command adb -ErrorAction SilentlyContinue
        if (-not $adbCommand) {
            $sdkRoots = @($env:ANDROID_HOME, $env:ANDROID_SDK_ROOT, (Join-Path $env:LOCALAPPDATA "Android\Sdk")) |
                Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
            foreach ($sdkRoot in $sdkRoots) {
                $candidate = Join-Path $sdkRoot "platform-tools\adb.exe"
                if (Test-Path $candidate) {
                    $adbCommand = Get-Item $candidate
                    break
                }
            }
        }
        if ($adbCommand) {
            $adbPath = if ($adbCommand.Source) { $adbCommand.Source } else { $adbCommand.FullName }
            & $adbPath reverse "tcp:$ApiPort" "tcp:$ApiPort"
            if ($LASTEXITCODE -eq 0) {
                Write-Host "Android USB bridge configured on tcp:$ApiPort." -ForegroundColor DarkGreen
            }
            else {
                Write-Warning "adb reverse failed. Connect the phone by USB with USB debugging enabled."
            }
        }
        else {
            Write-Warning "adb not found. Android emulator works, but a physical phone needs Android SDK platform-tools."
        }
    }

    Write-Host "Launching MAUI target $Framework ..." -ForegroundColor Cyan
    if ($Framework -like "net10.0-android*" -and $adbCommand) {
        & $dotnet publish $mauiProject -f $Framework -c Debug --no-restore
        if ($LASTEXITCODE -ne 0) {
            throw "MAUI publish failed with exit code $LASTEXITCODE."
        }

        $apk = Get-ChildItem (Join-Path $root "bin\Debug\$Framework\publish") -Filter "*-Signed.apk" |
            Sort-Object LastWriteTime -Descending |
            Select-Object -First 1
        if (-not $apk) {
            throw "Signed Android APK not found after publish."
        }

        & $adbPath install -r $apk.FullName
        if ($LASTEXITCODE -ne 0) {
            throw "APK install failed with exit code $LASTEXITCODE."
        }
        & $adbPath shell monkey -p com.mobilo.app 1
    }
    else {
        $buildArguments = @(
            "build", $mauiProject, "-t:Run", "-f", $Framework,
            "--no-restore"
        )
        & $dotnet @buildArguments
    }
    if ($LASTEXITCODE -ne 0) {
        throw "MAUI launch failed with exit code $LASTEXITCODE."
    }

    Write-Host "Mobilo is running. Keep this terminal open while using the app." -ForegroundColor Green
    [void](Read-Host "Press Enter after closing the app to stop the API")
}
finally {
    if ($api -and -not $api.HasExited) {
        Write-Host "Stopping Mobilo API ..." -ForegroundColor DarkGray
        Stop-Process -Id $api.Id -Force
    }
}
