@echo off
setlocal enabledelayedexpansion

echo ========================================================
echo       Starting Deep Clean (Deleting bin and obj)
echo ========================================================

set "ROOT_DIR=%~dp0"

set /a "DELETED_COUNT=0"

for /d /r "%ROOT_DIR%" %%d in (bin, obj, builds) do (
    if exist "%%d" (
        echo [DELETE] %%d
        rd /s /q "%%d"
        set /a "DELETED_COUNT+=1"
    )
)

echo ========================================================
if !DELETED_COUNT! equ 0 (
    echo [INFO] No build artifacts found. Already clean.
) else (
    echo [SUCCESS] Deleted !DELETED_COUNT! folders.
)
echo ========================================================

if "%1" neq "nopause" pause