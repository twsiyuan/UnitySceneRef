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
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneRef
{
    static SceneRef empty = null;

    [SerializeField]
    string sceneGUID;

    public SceneRef(string guid = "")
    {
        this.GUID = guid;
    }

    public static SceneRef Empty
    {
        get
        {
            if (empty == null)
            {
                empty = new SceneRef();
            }

            return empty;
        }
    }

    public string GUID
    {
        get
        {
            return this.sceneGUID;
        }

        set
        {
            this.sceneGUID = value;
        }
    }

    public string SceneName
    {
        get
        {
            var s = this.Scene;
            if (s == null)
            {
#if UNITY_EDITOR
                var path = AssetDatabase.GUIDToAssetPath(this.GUID);
                var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                if (asset != null)
                {
                    return asset.name;
                }
#endif
                return string.Empty;
            }

            return s.SceneName;
        }
    }

    public int BuildIndex
    {
        get
        {
            var s = this.Scene;
            return s == null ? -2 : s.BuildIndex;
        }
    }

    SceneRefSettings.Scene Scene
    {
        get
        {
            return SceneRefSettings.GetScene(this.sceneGUID);
        }
    }

    public static bool operator ==(SceneRef a, SceneRef b)
    {
        if (object.ReferenceEquals(a, b))
        {
            return true;
        }

        if (a == null || b == null)
        {
            return false;
        }

        return a.GUID == b.GUID;
    }

    public static bool operator !=(SceneRef a, SceneRef b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        var r = obj as SceneRef;
        if (this == null)
        {
            return false;
        }

        return this.GUID.Equals(r.GUID);
    }

    public bool Equals(SceneRef obj)
    {
        if (obj == null)
        {
            return false;
        }

        return this.GUID.Equals(obj.GUID);
    }

    public override int GetHashCode()
    {
        return this.GUID.GetHashCode();
    }
}
