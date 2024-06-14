using FFramework.Utils.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace FFramework.ViewMark.Editor
{

    [CustomEditor(typeof(ScriptMark))]

    public class ScriptMarkEditor : UnityEditor.Editor
    {

        SerializedProperty buildComponents;
        SerializedProperty buildProperties;
        SerializedProperty viewPath;
        SerializedProperty viewName;
        SerializedProperty viewNameSpace;
        SerializedProperty viewBaseType;
        SerializedProperty viewDescription;

        //能作为View基类的全部类
        IEnumerable<Type> viewTypes = null;
        static string[] viewTypesString;

        //全部组件（不含ScriptMark）
        List<Component> components;
        List<string> componentsString;
        List<string> componentsStringUnique;

        bool isRootMark = true;
        GameObject rootGameObject;

        static List<string> namepsaces = null;

        private void OnEnable()
        {
            buildComponents = serializedObject.FindProperty($"{nameof(ScriptMark.buildComponents)}");
            buildProperties = serializedObject.FindProperty($"{nameof(ScriptMark.buildProperties)}");



            //生成路径
            viewPath = serializedObject.FindProperty($"{nameof(ScriptMark.ViewPath)}");
            viewName = serializedObject.FindProperty($"{nameof(ScriptMark.ViewName)}");
            //基类
            viewBaseType = serializedObject.FindProperty($"{nameof(ScriptMark.ViewBaseType)}");
            //命名空间
            viewNameSpace = serializedObject.FindProperty($"{nameof(ScriptMark.ViewNameSpace)}");

            //描述
            viewDescription = serializedObject.FindProperty($"{nameof(ScriptMark.ViewDescription)}");

            //此物体的全部组件（不含ScriptMark）
            components = GetRoot()
                .GetComponents<Component>()
                .ToList();
            components.RemoveAll((x) => x.GetType() == typeof(ScriptMark));
            componentsString = components
                .Select((x) => x.GetType().GetDisplayName())
                .ToList();

            var result = componentsString.Select((x, i) =>
            {
                int count = componentsString.Take(i + 1).Where(y => y == x).Count();
                return count > 1 ? x + "_" + count : (i > 0 && componentsString[i - 1] == x ? x + "_" : x);
            });


            componentsStringUnique = result.ToList();


            //获取View基类
            if (viewTypes == null)
            {
                viewTypes = typeof(View)
                   .GetSubclassesOfIncludeThis((x) => x.GetCustomAttribute<ViewBaseAttribute>() != null && !x.IsGenericType);
                viewTypesString = viewTypes
                    .Select((x) => x.GetDisplayName())
                    .ToArray();

                Type viewBaseType = Type.GetType(this.viewBaseType.stringValue);
                if (viewBaseType != null)
                {
                    viewBaseTypeIndex = viewTypesString.ToList().IndexOf(viewBaseType.GetDisplayName());
                    if (viewBaseTypeIndex < 0) viewBaseTypeIndex = 0;
                }
                else viewBaseTypeIndex = 0;

            }


            //搜索命名空间
            namepsaces ??= AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany((x) => x.GetTypes())
                    .Where((x) => x.GetCustomAttribute<MVVMNamespaceAttribute>() != null)
                    .Select((x) => x.Namespace)
                    .Distinct()
                    .ToList();

            Debug.Log("OnEnable");
            //更新初始View名称
            if (viewName.stringValue == string.Empty)
            {
                viewName.stringValue = GetRoot().name;
            }


            GameObject parent = GetRoot();
            do
            {
                parent = parent.transform.parent?.gameObject;
                if (parent == null) break;
                if (parent.GetComponent<ScriptMark>() != null)
                {
                    isRootMark = false;
                    rootGameObject = parent;
                    break;
                }
            }
            while (parent != null);
        }


        int propertyMask = 0;
        int viewBaseTypeIndex = 0;


        private int[] componentsIndex;
        private Vector2 scrollPosition;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Only allow editing outside of runtime", MessageType.Info);
                return;
            }
            bool hasError = false;
            GUI.color = Color.green;
            if (GUILayout.Button("Add"))
            {
                buildComponents.InsertArrayElementAtIndex(buildComponents.arraySize);
                buildProperties.InsertArrayElementAtIndex(buildProperties.arraySize);
                SerializedProperty elementComponent = buildComponents.GetArrayElementAtIndex(buildComponents.arraySize - 1);
                SerializedProperty elementProperty = buildProperties.GetArrayElementAtIndex(buildProperties.arraySize - 1);
                elementComponent.objectReferenceValue = components[0];
                elementProperty.stringValue = "";
            }
            GUI.color = Color.white;
            GUILayout.Space(10);


            Vector2 leftTop = GUILayoutUtility.GetLastRect().min;
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            componentsIndex = new int[buildComponents.arraySize];

            HashSet<Type> boundTypes = new HashSet<Type>();

            if (buildComponents.arraySize == 0)
                EditorGUILayout.HelpBox("There are no components bound, click Add to add a new binding", MessageType.Info);


            for (int i = 0; i < buildComponents.arraySize; i++)
            {

                SerializedProperty elementComponent = buildComponents.GetArrayElementAtIndex(i);
                SerializedProperty elementProperty = buildProperties.GetArrayElementAtIndex(i);
                Component buildComponent = (Component)elementComponent.objectReferenceValue;
                if (buildComponent == null)
                {
                    buildComponents.DeleteArrayElementAtIndex(i);
                    buildProperties.DeleteArrayElementAtIndex(i);
                    i--;
                    continue;
                }
                Type componentType = buildComponent?.GetType();
                string buildProperty = elementProperty.stringValue;
                #region 组件

                int comIndex = buildComponent.gameObject.GetComponents(componentType).ToList().IndexOf(buildComponent) + 1;

                //恢复已经选择的组件Mask
                string current = comIndex == 1 ? $"{componentType.Name}" : $"{componentType.Name}_{comIndex}";
                componentsIndex[i] = Math.Clamp(componentsStringUnique.IndexOf(current), 0, components.Count - 1);



                //绘制组件选择
                componentsIndex[i] = EditorGUILayout.Popup("组件", componentsIndex[i], componentsStringUnique.ToArray());

                elementComponent.objectReferenceValue = components[componentsIndex[i]];


                if (!boundTypes.TryAdd(components[componentsIndex[i]].GetType()))
                {
                    hasError = true;
                    EditorGUILayout.HelpBox($"Binding the same type {components[componentsIndex[i]].GetType().GetDisplayName()} is not allowed", MessageType.Error);
                }
                #endregion

                Vector2 leftTopGroup = GUILayoutUtility.GetLastRect().min;


                #region 属性
                //反射可读写的成员变量（可能超过32个，Mask最多保存32个）
                List<string> pros = GetMemberStringsByReflection(buildComponent);
                //分割为选项字符串（把之前保存的 “A|B|C”切分成选项）（可能超过32个）
                List<string> options = PopUpMaskUtils.SplitToMaskOptions(buildProperty);

                const int groupSize = 32;
                //按32个分组
                List<List<string>> max32Pros =
                    Enumerable.Range(0, (int)Math.Ceiling((double)pros.Count / groupSize))
                         .Select(i => pros.Skip(i * groupSize).Take(groupSize).ToList())
                         .ToList();

                //在每一组内的ID
                List<List<int>> indexes = new List<List<int>>();
                max32Pros.ForEach((group) =>
                {
                    indexes.Add(PopUpMaskUtils.GetIndexFromString(group, options));
                });

                List<string> selectedOptions = new List<string>();
                for (int j = 0; j < indexes.Count; j++)
                {
                    //取得当前组的Mask值
                    propertyMask = PopUpMaskUtils.GetMaskFromIndex(indexes[j]);
                    //绘制MaskField
                    propertyMask = EditorGUILayout.MaskField($"属性", propertyMask, max32Pros[j].ToArray());
                    //从Mask获取索引值
                    indexes[j] = PopUpMaskUtils.GetIndexFromMask(propertyMask, max32Pros[j].Count);
                    //被选择的选项
                    selectedOptions.AddRange(PopUpMaskUtils.GetStringFromIndex(max32Pros[j], indexes[j]));
                }

                var savedStr = PopUpMaskUtils.MergeToMaskString(selectedOptions);
                elementProperty.stringValue = savedStr;

                #endregion


                if (GUILayout.Button("Remove"))
                {
                    buildComponents.DeleteArrayElementAtIndex(i);
                    buildProperties.DeleteArrayElementAtIndex(i);
                }



                Vector2 rightBottomGroup = GUILayoutUtility.GetLastRect().max;

                GUILayout.Space(10);

                GUI.depth = 1000;
                Color color = GUI.color;
                GUI.color = Color.black;
                GUI.Box(new Rect(leftTopGroup, rightBottomGroup), GUIContent.none);
                GUI.color = color;
                GUI.depth = 0;
            }
            EditorGUILayout.EndScrollView();

            if (!isRootMark)
            {
                serializedObject.ApplyModifiedProperties();
                return;
            }
            else if (rootGameObject != null)
            {
                if (GUILayout.Button("Focus Root"))
                    EditorGUIUtility.PingObject(rootGameObject);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            GUILayout.Space(10);

            //路径
            viewPath.stringValue = EditorGUILayout.TextField("ViewPath", viewPath.stringValue);
            string tempPath = viewPath.stringValue;
            EditorGUIHelper.DragPath(GUILayoutUtility.GetLastRect(), ref tempPath);
            viewPath.stringValue = tempPath;
            if (!EditorPathUtils.DirectoryExist(EPathType.AssetPath, viewPath.stringValue))
            {
                EditorGUILayout.HelpBox("不是有效的目录", MessageType.Error);
            }

            viewName.stringValue = EditorGUILayout.TextField(new GUIContent("ViewName"), viewName.stringValue);
            if (viewName.stringValue == string.Empty)
            {
                EditorGUILayout.HelpBox("不是有效的名称", MessageType.Error);
            }
            


            viewBaseTypeIndex = EditorGUILayout.Popup("Base", viewBaseTypeIndex, viewTypesString);
            viewBaseType.stringValue = viewTypes.ElementAt(viewBaseTypeIndex).AssemblyQualifiedName;
            int x = EditorGUILayout.Popup(namepsaces.IndexOf(viewNameSpace.stringValue), namepsaces.ToArray());
            viewNameSpace.stringValue = namepsaces[x];

            viewDescription.stringValue = EditorGUILayout.TextArea(viewDescription.stringValue, GUILayout.Height(100));


            if (GUILayout.Button("Generate"))
            {
                if (hasError)
                {
                    EditorUtility.DisplayDialog("Error", "Binding the same type is not allowed", "OK");
                    return;
                }
                Generate();
                //生成代码
            }


            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        void Generate()
        {
            if (!EditorPathUtils.DirectoryExist(EPathType.AssetPath, viewPath.stringValue))
                return;

            var marks = GetMarks(GetRoot());
            string className = viewName.stringValue;

            string viewCode = BuildViewCode(marks, viewNameSpace.stringValue, className, true);


            File.WriteAllText(
                Path.Combine(
                    EditorPathUtils.GetAbsLocation(EPathType.AssetPath, viewPath.stringValue),
                    $"{className}.cs")
                , viewCode);

            AssetDatabase.Refresh();

        }






        string BuildViewCode(List<ScriptMark> marks, string _namespace, string _className, bool _isPartial = true)
        {
            List<string> componentFieldsString = new List<string>();
            List<string> componentFindString = new List<string>();

            //遍历Mark
            foreach (var mark in marks)
            {
                for (int i = 0; i < mark.buildComponents.Count; i++)
                {
                    Component com = mark.buildComponents[i];
                    Type comType = com.GetType();
                    //原本的物体名
                    string originGameObjectName = com.gameObject.name;
                    string pattern = "[^a-zA-Z0-9_\u4e00-\u9fa5]";
                    //格式化物体名
                    string gameObjectName = Regex.Replace(originGameObjectName, pattern, "_");

                    //组件是第X个重复的
                    int comIndex = com.gameObject.GetComponents(comType).ToList().IndexOf(com) + 1;

                    //组件属性构建
                    string componentField =
                    $"public {comType.FullName} @{gameObjectName}_{comType.Name}{(comIndex > 1 ? $"_{comIndex}" : "")} {{ get; private set; }} = default;";

                    componentFieldsString.Add(componentField);

                    string componentInit = string.Empty;
                    //如果这个标记是根标记
                    if (mark.gameObject == GetRoot())
                    {
                        //组件初始化
                        componentInit =
                            $"@{gameObjectName}_{comType.Name}{(comIndex > 1 ? $"_{comIndex}" : "")} = this.gameObject.GetComponent<{com.GetType().FullName}>();";

                    }
                    else
                    {
                        //当前ScriptMark的Transform组件
                        Transform cur = com.transform;
                        string findPath = cur.gameObject.name;
                        //如果当前不是第一层子物体，则构建物体路径
                        while (cur.parent.gameObject != GetRoot())
                        {
                            findPath = $"{cur.parent.gameObject.name}/" + findPath;
                            cur = cur.parent;
                        }
                        componentInit =
                            $"@{gameObjectName}_{comType.Name}{(comIndex > 1 ? $"_{comIndex}" : "")} = transform.Find(@\"{findPath}\").GetComponent<{com.GetType().FullName}>();";

                    }
                    componentFindString.Add(componentInit);

                    var properties = PopUpMaskUtils.SplitToMaskOptions(mark.buildProperties[i]);
                    properties.RemoveAll((x) => x == string.Empty || x == null);
                    foreach (string property in properties)
                    {
                        var memberType = GetFieldOrPropertyTypeByName(property, com);
                        string filed =
                            $"public {nameof(FFramework.BindableProperty<int>)}<{memberType.FullName}> @{gameObjectName}_{com.GetType().Name}{(comIndex > 1 ? $"_{comIndex}" : "")}_{property}_{property} {{get;set;}}=default;";

                        string init =
                            $"@{gameObjectName}_{com.GetType().Name}{(comIndex > 1 ? $"_{comIndex}" : "")}_{property} = new {nameof(BindableProperty<int>)}<{memberType.FullName}>(@{gameObjectName}_{com.GetType().Name}{(comIndex > 1 ? $"_{comIndex}" : "")},(y)=>(({com.GetType().FullName})y).{property},(y,x)=>(({com.GetType().FullName})y).{property}=x);";

                        componentFieldsString.Add(filed);
                        componentFindString.Add(init);
                    }
                }
            }



            string model =
 @$"/*******************************************************
 * Code Generated By {nameof(FFramework)}
 * DateTime : #DATETIME#
 * UVersion : #VERSION#
 *******************************************************/
using UnityEngine;
using {nameof(FFramework)};

namespace #NAMESPACE#
{{

    /// <summary>
    /// {viewDescription.stringValue}
    /// </summary>
    public #PARTIAL#class #CLASS# : #BASECLASS#
    {{
#FIELDS#

        public void InitRefs()
        {{
#INIT#
        }}
    }}
}}
";
            Type viewBaseType = Type.GetType(this.viewBaseType.stringValue);
            string threeTab = "\t\t\t";
            string fourTab = "\t\t\t\t";
            string componentFieldsStringJoin = threeTab + string.Join(System.Environment.NewLine + threeTab, componentFieldsString);
            string componentFindStringJoin = fourTab + string.Join(System.Environment.NewLine + fourTab, componentFindString);


            Dictionary<string, string> replaceList = new Dictionary<string, string>()
            {
                { "#DATETIME#" , DateTime.UtcNow.ToString()},
                {"#VERSION#" ,Application.unityVersion.ToString()},
                {"#NAMESPACE#",_namespace },
                {"#CLASS#", _className },
                {"#PARTIAL#" , _isPartial ? "partial ":""},
                { "#BASECLASS#", viewBaseType.FullName},
                {"#FIELDS#", componentFieldsStringJoin },
                { "#INIT#", componentFindStringJoin}
            };
            //Debug.Log(viewBaseType.FullName);
            foreach (var item in replaceList)
                model = model.Replace(item.Key, item.Value);

            return model;
        }


        /// <summary>
        /// 从一个物体开始，搜索子ScriptMark
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        List<ScriptMark> GetMarks(GameObject gameObject)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(gameObject.transform);
            List<ScriptMark> marks = new List<ScriptMark>();
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                foreach (Transform t in node.transform)
                {
                    queue.Enqueue(t);
                }
                var mks = node.GetComponents<ScriptMark>();
                for (int i = 0; i < mks.Length; i++)
                {
                    marks.Add(mks[i]);
                }
            }
            return marks;
        }



        /// <summary>
        /// 反射获取对象的公开实例字段，以及可读可写的属性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<string> GetMemberStringsByReflection(object obj)
        {
            var fields = FindFields(obj);
            var properties = FindProperty(obj);
            var members = MergeInfos(fields, properties);
            var list = members.Select((x) => x.Name).ToList();

            return list;
        }


        /// <summary>
        /// 合并属性字段列表为成员列表
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static List<MemberInfo> MergeInfos(List<FieldInfo> f1, List<PropertyInfo> p1)
        {
            List<MemberInfo> members = new List<MemberInfo>();
            foreach (FieldInfo f2 in f1)
            {
                members.Add(f2);
            }
            foreach (PropertyInfo p2 in p1)
            {
                members.Add(p2);
            }
            return members;
        }

        /// <summary>
        /// 获取全部公开实例字段
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<FieldInfo> FindFields(object obj)
        {
            return FindFields(obj.GetType());
        }

        List<FieldInfo> FindFields(Type type)
        {
            FieldInfo[] fieldsArray = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            List<FieldInfo> fields = new List<FieldInfo>();

            foreach (FieldInfo p in fieldsArray)
            {
                if (p.IsLiteral || p.IsInitOnly) continue;
                fields.Add(p);
            }

            return fields;
        }

        /// <summary>
        /// 获取对象的全部公开实例可读可写属性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<PropertyInfo> FindProperty(object obj)
        {
            return FindProperty(obj.GetType());
        }
        List<PropertyInfo> FindProperty(Type type)
        {
            PropertyInfo[] pros = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<PropertyInfo> properties = new List<PropertyInfo>();
            foreach (PropertyInfo p in pros)
            {
                if (p.CanRead && p.CanWrite) properties.Add(p);
            }
            return properties;
        }

        /// <summary>
        /// 通过名字获取属性或字段
        /// </summary>
        /// <param name="name"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        Type GetFieldOrPropertyTypeByName(string name, object origin)
        {
            return GetFieldOrPropertyTypeByName(name, origin.GetType());
        }
        /// <summary>
        /// 通过名字获取属性或字段
        /// </summary>
        /// <param name="name"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        Type GetFieldOrPropertyTypeByName(string name, Type origin)
        {
            var fi = origin.GetField(name);
            var pi = origin.GetProperty(name);
            return fi?.FieldType ?? pi?.PropertyType;
        }

        public GameObject GetRoot()
        {
            return ((ScriptMark)serializedObject.targetObject).gameObject;
        }
    }
}