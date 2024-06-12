UNITY_PROJECT_PATH=../../UnityProject
UNITY_ASSET_PATH=$UNITY_PROJECT_PATH/Assets
PROJECT_PATH=../..
CONF_PATH=.

OUT_PATH=$UNITY_PROJECT_PATH/Assets\Res/Text/DataTable
OUT_CODE_PATH=$UNITY_PROJECT_PATH/Assets/Scripts/DataTable/Codes
LUBAN_DLL=$CONF_PATH/Luban/Luban.dll

dotnet $LUBAN_DLL \
    -t client \
    -f \
    --conf $CONF_PATH/luban.conf \
    -x pathValidator.rootDir=$UNITY_PROJECT_PATH


pause