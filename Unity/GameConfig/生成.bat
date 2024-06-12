set UNITY_PROJECT_PATH=..
set UNITY_ASSET_PATH=%UNITY_PROJECT_PATH%\Assets
set CONF_PATH=.

set OUT_PATH=%UNITY_ASSET_PATH%\Res\Config\
set OUT_CODE_PATH=%UNITY_ASSET_PATH%\Scripts\Config\Code
set LUBAN_DLL=%CONF_PATH%\Luban\Luban.dll

dotnet %LUBAN_DLL% ^
    -t client ^
    -d bin ^
    -c cs-bin ^
    --conf %CONF_PATH%\luban.conf ^
    -x outputCodeDir=%OUT_CODE_PATH% ^
    -x outputDataDir=%OUT_PATH%^
    -x pathValidator.rootDir=%UNITY_PROJECT_PATH%


pause