@SET CONF=%1
@if not "%CONF%"=="" goto set_IDE
@SET CONF=Release

:set_IDE
@if not "%VS100IDE%"=="" goto run_build

@if not "%VS100COMNTOOLS%"=="" set VS100IDE=%VS100COMNTOOLS:~0,-6%IDE\

@if not "%VS100IDE%"=="" goto run_build

@SET VS100IDE=C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\

@if not "%ProgramFiles(x86)%" == "" SET VS100IDE=%ProgramFiles(x86)%\Microsoft Visual Studio 10.0\Common7\IDE\

:run_build
"%VS100IDE%\devenv.com" Tp.Integration.Ide.VisualStudio.sln /rebuild "%CONF%" /project Tp.Integration.Ide.VisualStudio.SetUp\Tp.Integration.Ide.VisualStudio.SetUp.vdproj /projectconfig "%CONF%"
