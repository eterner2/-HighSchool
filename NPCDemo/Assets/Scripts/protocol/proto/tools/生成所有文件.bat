@echo off

set SRC_DIR=..
set DST_DIR=..\..

::遍历所有文件
for /f "delims=" %%i in ('dir /b "%SRC_DIR%\*.proto"') do (
	echo .\protoc.exe -I=%SRC_DIR% --csharp_out=%DST_DIR% %%i
	.\protoc.exe -I=%SRC_DIR% --csharp_out=%DST_DIR% %%i
)

echo 协议生成完毕。
 
pause