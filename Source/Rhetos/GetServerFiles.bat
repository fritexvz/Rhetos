SET Config=%1%
IF [%1] == [] SET Config=Debug

REM Set current working folder to folder where this script is, to ensure that the relative paths in this script are valid.
SET ThisScriptFolder=%~dp0
PUSHD %ThisScriptFolder%

SET BinFolder="%CD%\bin"
SET PluginsFolder="%CD%\Plugins"
SET ResourcesFolder="%CD%\Resources"
SET DslScriptsFolder="%CD%\DslScripts"

REM ======================== REST SERVICE ==============================

XCOPY /Y/D/R ..\..\Source\Rhetos.Security.Service\bin\%Config%\*.dll %BinFolder% || EXIT /B 1

REM ======================== DEPLOYMENT TOOLS ==============================

XCOPY /Y/D/R ..\..\Source\DeployPackages\bin\%Config%\DeployPackages.??? %BinFolder% || EXIT /B 1
XCOPY /Y/D/R ..\..\Source\DeployPackages\bin\%Config%\DeployPackages.exe.config %BinFolder% || EXIT /B 1
XCOPY /Y/D/R ..\..\Source\DeployPackages\bin\%Config%\*.dll %BinFolder% || EXIT /B 1
XCOPY /Y/D/R ..\..\Source\DeployPackages\bin\%Config%\*.pdb %BinFolder%

XCOPY /Y/D/R ..\..\Source\ExtractPackages\bin\%Config%\ExtractPackages.??? %BinFolder% || EXIT /B 1
XCOPY /Y/D/R ..\..\Source\ExtractPackages\bin\%Config%\*.dll %BinFolder% || EXIT /B 1
XCOPY /Y/D/R ..\..\Source\ExtractPackages\bin\%Config%\*.pdb %BinFolder%

XCOPY /Y/D/R ..\..\Source\CleanupOldData\bin\%Config%\CleanupOldData.??? %BinFolder% || EXIT /B 1
XCOPY /Y/D/R ..\..\Source\CleanupOldData\bin\%Config%\CleanupOldData.exe.config %BinFolder% || EXIT /B 1
XCOPY /Y/D/R ..\..\Source\CleanupOldData\bin\%Config%\*.dll %BinFolder% || EXIT /B 1
XCOPY /Y/D/R ..\..\Source\CleanupOldData\bin\%Config%\*.pdb %BinFolder%

REM ========================
POPD

ECHO Done.
