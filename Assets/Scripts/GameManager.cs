using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Over UI")]
    public GameObject gameOverPanel;  // kéo GameOverPanel vào đây

    bool isGameOver = false;

    void Awake()
    {
        // Thiết lập singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Ẩn panel lúc đầu
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// Gọi khi player mất hết máu
    /// </summary>
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Hiện UI
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Dừng mọi chuyển động vật lý và Update
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Ví dụ hàm Restart Level từ button
    /// </summary>
    // Gọi từ nút Restart
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Gọi từ nút Main Menu
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // scene index 0 = MainMenu
    }
}
