using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SceneRef))]
public class SceneRefPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = EditorGUIUtility.singleLineHeight;
        var guidProperty = property.FindPropertyRelative("sceneGUID");
        var guid = guidProperty.stringValue;

        if (!string.IsNullOrEmpty(guid))
        {
            var build = EditorBuildSettings.scenes.Where(s => s.guid.ToString() == guid).FirstOrDefault();
            if (build == null || !build.enabled)
            {
                height += EditorGUIUtility.singleLineHeight * 2;
            }
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var guidProperty = property.FindPropertyRelative("sceneGUID");
        var rect = position;
        rect.height = EditorGUIUtility.singleLineHeight;

        EditorGUI.BeginProperty(rect, label, property);

        var guid = guidProperty.stringValue;
        var path = AssetDatabase.GUIDToAssetPath(guid);
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

        EditorGUI.BeginChangeCheck();

        if (label.text == guid && oldScene != null)
        {
            label = new GUIContent(oldScene.name);
        }

        var newScene = EditorGUI.ObjectField(rect, label, oldScene, typeof(SceneAsset), false) as SceneAsset;
        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var newGuid = AssetDatabase.AssetPathToGUID(newPath);
            guidProperty.stringValue = newGuid;
        }

        EditorGUI.EndProperty();

        if (!string.IsNullOrEmpty(guid))
        {
            const float ButtonWidth = 100f;
            var build = EditorBuildSettings.scenes.Where(s => s.guid.ToString() == guid).FirstOrDefault();

            var helpRect = position;
            helpRect.y += EditorGUIUtility.singleLineHeight;
            helpRect.width -= ButtonWidth;
            helpRect.height = EditorGUIUtility.singleLineHeight * 2;

            var buttonRect = position;
            buttonRect.x = helpRect.x + helpRect.width;
            buttonRect.y = helpRect.y + (EditorGUIUtility.singleLineHeight * .5f);
            buttonRect.width = ButtonWidth;
            buttonRect.height = EditorGUIUtility.singleLineHeight;

            if (build == null)
            {
                EditorGUI.HelpBox(helpRect, "Scene is not in build", MessageType.Warning);
                if (GUI.Button(buttonRect, "Add scene"))
                {
                    GUID unityGUID;
                    GUID.TryParse(guid, out unityGUID);
                    var s = EditorBuildSettings.scenes;
                    ArrayUtility.Add(ref s, new EditorBuildSettingsScene
                    {
                        enabled = true,
                        guid = unityGUID,
                        path = path,
                    });

                    EditorBuildSettings.scenes = s;
                }
            }
            else if (!build.enabled)
            {
                EditorGUI.HelpBox(helpRect, "Scene is not enabled", MessageType.Warning);
                if (GUI.Button(buttonRect, "Make enabled"))
                {
                    var s = EditorBuildSettings.scenes;
                    s.Where(v => v.guid.ToString() == guid).First().enabled = true;
                    EditorBuildSettings.scenes = s;
                }
            }
        }
    }
}