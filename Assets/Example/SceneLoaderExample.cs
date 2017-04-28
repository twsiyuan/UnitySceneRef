using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderExample : MonoBehaviour
{
    [SerializeField]
    SceneRef[] scenes;

    void Start()
    {
        foreach (var s in this.scenes)
        {
            if (s.IsValid)
            {
                SceneManager.LoadScene(s.SceneName, LoadSceneMode.Additive);
            }
            else
            {
                Debug.LogWarningFormat("Scene '{0}' is not in build settings", s.SceneName);
            }
        }
	}
}
