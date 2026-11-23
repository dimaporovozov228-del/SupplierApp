@echo off
setlocal
set "SUPPLIER_PS=%SystemRoot%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe"
if not exist "%SUPPLIER_PS%" set "SUPPLIER_PS=%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe"
"%SUPPLIER_PS%" -NoProfile -File "%~dp0CreateDatabase.ps1" %*
if errorlevel 1 pause
endlocal
