using NaughtyAttributes;
using UC;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickToScene : MonoBehaviour
{
    [SerializeField, Scene] private string sceneName;
    [SerializeField] private bool disableCursor;

    private void Start()
    {
        if (disableCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

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
