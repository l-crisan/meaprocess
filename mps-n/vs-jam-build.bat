set SolutionDir=%~dp0
set Config=%1
set ProjectName=%2
set Top=%SolutionDir%
set GCC="C:\sca\gcc-linaro-7.3.1-ARM32\bin\arm-linux-gnueabihf-"
set GCC64="C:\sca\gcc-linaro-7.3.1-ARM64\bin\aarch64-linux-gnu-"

cd %Top%

if "%Config%" == "LinuxARMDebug" (
call jam.bat configure --debug -sCONFIG=%Config% -sTOOLSET=gcc -sTARGET_OSPLAT=arm -sTOOLSET_ROOT=%GCC% -sTARGET_OS=linux
)

if "%Config%" == "LinuxARMRelease" (
call jam.bat configure --optimize -sCONFIG=%Config% -sTOOLSET=gcc -sTARGET_OSPLAT=arm -sTOOLSET_ROOT=%GCC% -sTARGET_OS=linux
)

if "%Config%" == "LinuxARM64Debug" (
call jam.bat configure --debug -sCONFIG=%Config% -sTOOLSET=gcc -sTARGET_OSPLAT=arm -sTOOLSET_ROOT=%GCC64% -sTARGET_OS=linux
)

if "%Config%" == "LinuxARM64Release" (
call jam.bat configure --optimize -sCONFIG=%Config% -sTOOLSET=gcc -sTARGET_OSPLAT=arm -sTOOLSET_ROOT=%GCC64% -sTARGET_OS=linux
)

if "%Config%" == "WinX86Debug" (
call jam.bat configure --debug -sCONFIG=%Config%  -sTARGET_OSPLAT=x86  -sTOOLSET=vc16
)

if "%Config%" == "WinX86Release" (
call jam.bat configure --optimize -sCONFIG=%Config%  -sTARGET_OSPLAT=x86 -sTOOLSET=vc16
)

if "%Config%" == "WinX64Debug" (
call jam.bat configure --debug -sCONFIG=%Config%  -sTARGET_OSPLAT=x86_64 -sTOOLSET=vc16
)

if "%Config%" == "WinX64Release" (
call jam.bat configure --optimize -sCONFIG=%Config%  -sTARGET_OSPLAT=x86_64 -sTOOLSET=vc16
)

call jam.bat -sTOP=%Top% -q -j4  %ProjectName% 
