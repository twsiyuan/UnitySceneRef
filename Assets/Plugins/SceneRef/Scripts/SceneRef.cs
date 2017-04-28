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

    public bool IsValid
    {
        get
        {
            return this.BuildIndex >= 0;
        }
    }

    public int BuildIndex
    {
        get
        {
            var s = this.Scene;
            return s == null ? -1 : s.BuildIndex;
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
