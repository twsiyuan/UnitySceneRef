using UnityEditor.Build;
using UnityEditor;

public class SceneRefBuildPreprocess : IPreprocessBuild
{
    public int callbackOrder
    {
        get
        {
            return 0;
        }
    }

    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        SceneRefSettings.Build();
    }
}
