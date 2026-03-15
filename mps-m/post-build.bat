rem DEBUG
copy ..\mps-n\build\WinX64Debug\*.dll .\Build\Debug\rt
copy ..\mps-n\lib\WinX64Debug\*.dll .\Build\Debug\rt

copy ..\mps-n\build\WinX64Debug\mpal-debugger.exe .\Build\Debug\
copy ..\mps-n\build\WinX64Debug\mpal-vm.dll .\Build\Debug\
copy ..\mps-n\build\WinX64Debug\SGLW64.dll .\Build\Debug\
copy ..\mps-n\build\WinX64Debug\mps-licence.dll .\Build\Debug\
copy ..\mps-n\build\WinX64Debug\mps-drv-opencv.dll .\Build\Debug\
copy ..\mps-n\build\WinX64Debug\mps-drv-audio.dll .\Build\Debug\

copy ..\mps-n\lib\WinX64Debug\Pt.dll .\Build\Debug\
copy ..\mps-n\lib\WinX64Debug\Pt-System.dll .\Build\Debug\
copy ..\mps-n\lib\WinX64Debug\Pt-Net.dll .\Build\Debug\
copy ..\mps-n\lib\WinX64Debug\opencv_*.dll .\Build\Debug\
copy ..\mps-n\lib\WinX64Debug\portaudio*.dll .\Build\Debug\

rem RELEASE
copy ..\mps-n\build\WinX64Release\*.dll .\Build\Release\rt
copy ..\mps-n\lib\WinX64Release\*.dll .\Build\Release\rt

copy ..\mps-n\build\WinX64Release\mpal-debugger.exe .\Build\Release\
copy ..\mps-n\build\WinX64Release\mpal-vm.dll .\Build\Release\
copy ..\mps-n\build\WinX64Release\SGLW64.dll .\Build\Release\
copy ..\mps-n\build\WinX64Release\mps-licence.dll .\Build\Release\
copy ..\mps-n\build\WinX64Release\mps-drv-opencv.dll .\Build\Release\
copy ..\mps-n\build\WinX64Release\mps-drv-audio.dll .\Build\Release\


copy ..\mps-n\lib\WinX64Release\Pt.dll .\Build\Release\
copy ..\mps-n\lib\WinX64Release\Pt-System.dll .\Build\Release\
copy ..\mps-n\lib\WinX64Release\Pt-Net.dll .\Build\Release\
copy ..\mps-n\lib\WinX64Release\opencv_*.dll .\Build\Release\
copy ..\mps-n\lib\WinX64Release\portaudio*.dll .\Build\Release\

rem Resources
xcopy ..\mps-n\build\WinX64Debug\*.mres .\Build\Debug\rt /E
xcopy ..\mps-n\build\WinX64Release\*.mres .\Build\Release\rt /E