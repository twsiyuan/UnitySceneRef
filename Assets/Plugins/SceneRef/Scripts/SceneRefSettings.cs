// <copyright file="LICENSE.txt" company="Siyuan Wang">
//    Copyright 2017 Siyaun Wang. https://github.com/twsiyuan/.
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//    
//        http://www.apache.org/licenses/LICENSE-2.0
//    
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
#endif

public class SceneRefSettings : ScriptableObject
{
    const string ResourceName = "SceneRef";
    const string ResourcePath = "Assets/Resources/" + ResourceName + ".asset";

    static SceneRefSettings current = null;

    [SerializeField]
    Scene[] scenes = new Scene[0];

    public static Scene GetScene(string guid)
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            return new Scene
            {
                GUID = guid,
                SceneName = Path.GetFileNameWithoutExtension(path),
                BuildIndex = SceneUtility.GetBuildIndexByScenePath(path),
            };
        }

        if (current == null)
        {
            // Always rebuild when into play mode
            SceneRefSettings.Build();
        }
#endif

        if (current == null)
        {
            current = Resources.Load<SceneRefSettings>(ResourceName);

            if (current == null)
            {
                throw new System.InvalidOperationException("Cannnot load resource " + ResourceName);
            }
        }

        foreach (var s in current.scenes)
        {
            if (s.GUID == guid)
            {
                return s;
            }
        }

        return null;
    }

#if UNITY_EDITOR
    public static void Build()
    {
        var scenes = new List<SceneRefSettings.Scene>();
        foreach (var s in EditorBuildSettings.scenes)
        {
            scenes.Add(new SceneRefSettings.Scene()
            {
                GUID = s.guid.ToString(),
                BuildIndex = SceneUtility.GetBuildIndexByScenePath(s.path),
                SceneName = Path.GetFileNameWithoutExtension(s.path),
            });
        }

        var data = ScriptableObject.CreateInstance<SceneRefSettings>();
        data.scenes = scenes.ToArray();

        var folder = Path.GetDirectoryName(Path.GetFullPath(ResourcePath));
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        AssetDatabase.CreateAsset(data, ResourcePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif

    [System.Serializable]
    public class Scene
    {
        [SerializeField]
        string sceneName;

        [SerializeField]
        string sceneGUID;

        [SerializeField]
        int sceneBuildIndex;

        public string GUID
        {
            get
            {
                return this.sceneGUID;
            }

            internal set
            {
                this.sceneGUID = value;
            }
        }

        public string SceneName
        {
            get
            {
                return this.sceneName;
            }

            internal set
            {
                this.sceneName = value;
            }
        }

        public int BuildIndex
        {
            get
            {
                return this.sceneBuildIndex;
            }

            internal set
            {
                this.sceneBuildIndex = value;
            }
        }
    }
}
