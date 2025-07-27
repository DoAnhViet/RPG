using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Gọi từ OnClick của StartButton
    public void PlayGame()
    {
        // Load scene index 1 (GameScene)
        SceneManager.LoadScene(1);
    }

    // Gọi từ OnClick của QuitButton
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
