@ECHO OFF
SETLOCAL

set BUILD_DIR="builds"
set CLIENT_EXE="IO.Processor.Client.Learning.exe"
set SERVER_EXE="IO.Processor.Server.Learning.exe"

if not exist "%~dp0\%BUILD_DIR%\%CLIENT_EXE%" (
    ECHO [ERROR] %CLIENT_EXE% not found in %BUILD_DIR%
    pause
    exit /b 1
)

ECHO [LOG] Found %CLIENT_EXE% in %BUILD_DIR%

if not exist "%~dp0\%BUILD_DIR%\%SERVER_EXE%" (
    ECHO [ERROR] %SERVER_EXE% not found in %BUILD_DIR%
    pause
    exit /b 1
)

ECHO [LOG] Found %SERVER_EXE% in %BUILD_DIR%

START "Pipeline Server" /D "%~dp0\%BUILD_DIR%" cmd /k "%SERVER_EXE%"
START "Pipeline Client" /D "%~dp0\%BUILD_DIR%" cmd /k "%CLIENT_EXE%"

ECHO [LOG] Both processes launched.
ENDLOCAL