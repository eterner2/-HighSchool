@echo off

set FILE_NAME=cli_gg_msg.proto
set SRC_DIR=..
set DST_DIR=..\..


.\protoc.exe -I=%SRC_DIR% --csharp_out=%DST_DIR% %FILE_NAME%


echo ����%FILE_NAME%��ϡ�
 
pause