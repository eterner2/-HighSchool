@echo off

set FILE_NAME=cli_gg_msg.proto
set SRC_DIR=..
set DST_DIR=..\..


.\protoc.exe -I=%SRC_DIR% --csharp_out=%DST_DIR% %FILE_NAME%


echo Éú³É%FILE_NAME%Íê±Ï¡£
 
pause