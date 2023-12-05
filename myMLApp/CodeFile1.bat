@echo off
setlocal enabledelayedexpansion

for /F "tokens=*" %%A in (C:\test\Index.txt) do (
    set line =%% A
    echo(!line:~1! >> C:\test\Index1.txt
)