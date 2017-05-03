# UnitySceneRef

A simple Scene reference implementation in Unity

## How to install

Download and copy [/Assets/Plugins/SceneRef/\*.\*](/Assets/Plugins/SceneRef/) into your unty project, Unity 5.6.x or above.

## How to use

An example:

```
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
```

editor:

![Editor](/Previews/editor.png)

## License

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

[http://www.apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

