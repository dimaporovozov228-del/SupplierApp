@echo off
chcp 65001 >nul
cd /d "%~dp0"
if exist "SupplierApp\bin\Release\SupplierApp.exe" (
    start "" "SupplierApp\bin\Release\SupplierApp.exe"
    exit /b
)
if exist "SupplierApp\bin\Debug\SupplierApp.exe" (
    start "" "SupplierApp\bin\Debug\SupplierApp.exe"
    exit /b
)
echo Откройте SupplierApp.sln в Visual Studio и соберите проект для x86.
pause
