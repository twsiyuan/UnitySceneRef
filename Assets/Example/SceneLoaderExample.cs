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
            SceneManager.LoadScene(s.SceneName, LoadSceneMode.Additive);
        }
	}
}
