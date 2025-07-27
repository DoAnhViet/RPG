using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pauseMenu;
    public GameObject panelSaveSlots;

    bool isPaused = false;  // trạng thái toggle

    void Start()
    {
        pauseMenu.SetActive(false);
        panelSaveSlots.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    /// <summary>
    /// Gọi khi nhấn ESC hoặc nút PauseButton
    /// </summary>
    public void TogglePause()
    {
        // Nếu đang mở bảng save thì đóng nó về pauseMenu
        if (panelSaveSlots.activeSelf)
        {
            panelSaveSlots.SetActive(false);
            pauseMenu.SetActive(true);
            isPaused = true;
            return;
        }

        // Ngược lại toggle pauseMenu
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        // Khi đóng pauseMenu, chắc chắn panelSaveSlots cũng đóng
        if (!isPaused)
            panelSaveSlots.SetActive(false);

        // Dừng/tái chạy game
        Time.timeScale = isPaused ? 0f : 1f;
    }

    // Gọi từ nút Resume
    public void Resume()
    {
        TogglePause();
    }

    // Gọi từ nút Save Game
    public void OpenSave()
    {
        panelSaveSlots.SetActive(true);
        pauseMenu.SetActive(false);
        isPaused = true;
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
