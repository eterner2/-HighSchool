@echo off

set SRC_DIR=..
set DST_DIR=..\..

::���������ļ�
for /f "delims=" %%i in ('dir /b "%SRC_DIR%\*.proto"') do (
	echo .\protoc.exe -I=%SRC_DIR% --csharp_out=%DST_DIR% %%i
	.\protoc.exe -I=%SRC_DIR% --csharp_out=%DST_DIR% %%i
)

echo Э��������ϡ�
 
pause