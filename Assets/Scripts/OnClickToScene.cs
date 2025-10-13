using NaughtyAttributes;
using UC;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickToScene
    : MonoBehaviour
{
    [SerializeField, Scene] private string sceneName;


    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            FullscreenFader.FadeOut(1.0f, Color.black,
                () =>
                {
                    SceneManager.LoadScene(sceneName);
                });
        }
    }
}
