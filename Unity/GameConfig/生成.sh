UNITY_PROJECT_PATH=../../UnityProject
UNITY_ASSET_PATH=$UNITY_PROJECT_PATH/Assets
PROJECT_PATH=../..
CONF_PATH=.

OUT_PATH=$UNITY_PROJECT_PATH/Assets/Res/Text/DataTable
OUT_CODE_PATH=$UNITY_PROJECT_PATH/Assets/Scripts/DataTable/Codes
LUBAN_DLL=$CONF_PATH/Luban/Luban.dll

dotnet $LUBAN_DLL \
    -t client \
    -d bin \
    -c cs-bin \
    --conf $CONF_PATH/luban.conf \
    -x outputCodeDir=$OUT_CODE_PATH \
    -x outputDataDir=$OUT_PATH \
    -x pathValidator.rootDir=$UNITY_PROJECT_PATH


pause