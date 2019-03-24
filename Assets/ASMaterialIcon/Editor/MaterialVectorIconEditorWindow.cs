#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ASMaterialIcon
{
    class MaterialVectorIconEditorWindow : EditorWindow
    {
        private string searchInput;
        private Vector2 scrollPos = Vector2.zero;

        public static void ShowWindow()
        {
            GetWindow<MaterialVectorIconEditorWindow>(true, "Material Vector Icon Picker", true);
        }

        void OnGUI()
        {
            DrawSearchField();
            DrawIconPicker();
        }

        private void DrawSearchField()
        {
            EditorGUILayout.BeginHorizontal();
            searchInput = EditorGUILayout.TextField(searchInput, GUI.skin.FindStyle("ToolbarSeachTextField"));
            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                searchInput = "";
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawIconPicker()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            Dictionary<string, string> icons = MaterialVectorIcon.GetIcons();
            List<string> iconNameList = new List<string>(icons.Keys);
            if (!string.IsNullOrEmpty(searchInput))
            {
                for (int i = 0; i < iconNameList.Count; i++)
                {
                    if (!iconNameList[i].ToLower().Contains(searchInput.ToLower()))
                    {
                        iconNameList.RemoveAt(i);
                        i--;
                    }
                }
            }
            DrawIcons(icons, iconNameList);

            EditorGUILayout.EndScrollView();
        }

        private void DrawIcons(Dictionary<string, string> icons, List<string> iconNameList)
        {
            string name;
            float elementWidth = 50;
            int index = 0;
            GUIStyle iconStyle = new GUIStyle(GUI.skin.button);
            iconStyle.font = Resources.Load<Font>("ASMaterialIcon/Fonts/MaterialIcons-Regular") as Font;
            iconStyle.border = new RectOffset(5, 5, 5, 5);
            iconStyle.contentOffset = new Vector2(0, 0);
            iconStyle.alignment = TextAnchor.MiddleCenter;
            iconStyle.normal.textColor = (EditorGUIUtility.isProSkin ? Color.white : new Color(0.2f, 0.2f, 0.2f));

            while (iconNameList.Count > 0)
            {
                index++;
                name = iconNameList[0];
                iconNameList.RemoveAt(0);

                EditorGUILayout.BeginHorizontal();
                float remainingSpace = EditorGUIUtility.currentViewWidth - EditorGUIUtility.currentViewWidth / 10;
                while (iconNameList.Count > 0 && remainingSpace > elementWidth)
                {
                    Color oldColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.clear;
                    if (Selection.gameObjects != null)
                    {
                        for (int i = 0; i < Selection.gameObjects.Length; i++)
                        {
                            GameObject go = Selection.gameObjects[i];
                            if (go.GetComponent<MaterialVectorIcon>() != null)
                            {
                                MaterialVectorIcon icon = go.GetComponent<MaterialVectorIcon>();
                                if (name.Equals(icon.iconName))
                                {
                                    GUI.backgroundColor = new Color(1, 1, 1, 0.5f);
                                }
                            }
                        }
                    }
                    if (GUILayout.Button(new GUIContent(MaterialVectorIcon.Decode("\\u" + icons[name]), "Icon: " + name), iconStyle, GUILayout.Width(elementWidth), GUILayout.Height(50)))
                    {
                        if (Selection.gameObjects != null)
                        {
                            for (int i = 0; i < Selection.gameObjects.Length; i++)
                            {
                                GameObject go = Selection.gameObjects[i];
                                if (go.GetComponent<MaterialVectorIcon>() != null)
                                {
                                    MaterialVectorIcon icon = go.GetComponent<MaterialVectorIcon>();
                                    Undo.RecordObject(icon, "Changed icon of " + icon.name);
                                    icon.text = MaterialVectorIcon.Decode("\\u" + icons[name]);
                                    icon.iconName = name;
                                    EditorUtility.SetDirty(icon);
                                }
                            }
                        }
                    }
                    GUI.backgroundColor = oldColor;
                    remainingSpace -= elementWidth;
                    index++;
                    name = iconNameList[0];
                    iconNameList.RemoveAt(0);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
#endif