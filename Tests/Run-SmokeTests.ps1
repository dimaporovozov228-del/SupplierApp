$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
if (!(Test-Path -LiteralPath $vswhere)) { throw 'Install Visual Studio with .NET desktop development and .NET Framework 4.8 targeting pack.' }
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
if (!$msbuild) { throw 'MSBuild not found.' }
& $msbuild (Join-Path $projectRoot 'SupplierApp.sln') /t:Build /p:Configuration=Release /p:Platform=x86 /nologo /verbosity:minimal
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
$testBin = Join-Path $PSScriptRoot 'bin'
New-Item -ItemType Directory -Path $testBin -Force | Out-Null
$app = Join-Path $projectRoot 'SupplierApp\bin\Release\SupplierApp.exe'
Copy-Item -LiteralPath $app -Destination $testBin -Force
Copy-Item -LiteralPath ($app + '.config') -Destination (Join-Path $testBin 'SmokeTests.exe.config') -Force
$runner = Join-Path $testBin 'SmokeTests.exe'
$compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
& $compiler /nologo /target:exe /platform:x86 ("/out:" + $runner) ("/reference:" + $app) /reference:System.Data.dll /reference:System.Windows.Forms.dll /reference:System.Drawing.dll (Join-Path $PSScriptRoot 'SmokeTests.cs')
if ($LASTEXITCODE -ne 0) { throw 'Test compilation failed.' }
& $runner (Join-Path $projectRoot 'SupplierApp\Database\SupplierApp.mdb')
if ($LASTEXITCODE -ne 0) { throw 'Smoke tests failed.' }
