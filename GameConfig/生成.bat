set UNITY_PROJECT_PATH=../Unity
set UNITY_ASSET_PATH=%UNITY_PROJECT_PATH%\Assets
set CONF_PATH=.
set OUT_DATA_PATH=%UNITY_ASSET_PATH%\Res\Config\
set OUT_CODE_PATH=%UNITY_ASSET_PATH%\Scripts\Config\Code
set LUBAN_DLL=%CONF_PATH%\Luban\Luban.dll
set LUBAN_CONF_PATH=%CONF_PATH%\luban.conf 

dotnet %LUBAN_DLL% ^
    -t client ^
    -d bin ^
    -c cs-bin ^
    --conf %LUBAN_CONF_PATH%^
    -x outputCodeDir=%OUT_CODE_PATH% ^
    -x outputDataDir=%OUT_DATA_PATH%^
    -x pathValidator.rootDir=%UNITY_PROJECT_PATH% ^
    -x l10n.provider=default ^
    -x l10n.textFile.path=Data\Languages.xlsx ^
    -x l10n.textFile.keyFieldName=key 

pause