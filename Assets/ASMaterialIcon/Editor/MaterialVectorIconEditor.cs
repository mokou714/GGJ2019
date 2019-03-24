#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace ASMaterialIcon
{
    [CustomEditor(typeof(MaterialVectorIcon)), CanEditMultipleObjects]
    public class MaterialVectorIconEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MaterialVectorIcon icon = target as MaterialVectorIcon;
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Color"));

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Icon", GUILayout.Width(EditorGUIUtility.labelWidth));

            GUIStyle iconStyle = new GUIStyle(GUI.skin.label);
            iconStyle.font = icon.font;
            iconStyle.border = new RectOffset(5, 5, 5, 5);
            iconStyle.contentOffset = new Vector2(0, 0);
            iconStyle.alignment = icon.alignment;
            iconStyle.normal.textColor = (EditorGUIUtility.isProSkin ? Color.white : new Color(0.2f, 0.2f, 0.2f));

            if (Selection.gameObjects.Length < 2)
            {
                GUILayout.Label(new GUIContent(MaterialVectorIcon.Decode(icon.text), "Icon: " + icon.iconName), iconStyle, GUILayout.Width(20));
            }
            if (GUILayout.Button("Choose Icon"))
            {
                MaterialVectorIconEditorWindow.ShowWindow();
            }

            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem("GameObject/UI/Material Vector Icon")]
        public static void CreateMaterialVectorIcon()
        {
            GameObject go = new GameObject("Icon");
            go.AddComponent<MaterialVectorIcon>();

            GameObject selectedObj = Selection.activeGameObject;
            if (selectedObj != null)
            {
                go.transform.SetParent(selectedObj.transform, false);
            }
            Selection.activeGameObject = go;
        }

        [MenuItem("Component/UI/Material Vector Icon")]
        public static void AppendMaterialVectorIconComponent()
        {
            if (Selection.activeGameObject != null)
                Selection.activeGameObject.AddComponent<MaterialVectorIcon>();
        }

        [MenuItem("Component/UI/Material Vector Icon", true)]
        public static bool ValidateAppendMaterialVectorIconComponent()
        {
            return Selection.activeTransform != null;
        }
    }
}
#endif
