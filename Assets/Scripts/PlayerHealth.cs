using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text healthText;

    void OnEnable()
    {
        // Đăng ký sự kiện để update UI khi HP thay đổi
        StatsManager.OnHealthChanged += UpdateHealthUI;
    }

    void OnDisable()
    {
        StatsManager.OnHealthChanged -= UpdateHealthUI;
    }

    void Start()
    {
        // Khởi tạo UI
        UpdateHealthUI(StatsManager.Instance.currentHealth, StatsManager.Instance.maxHealth);
    }

    void UpdateHealthUI(int current, int max)
    {
        if (healthText != null)
            healthText.text = $"HP: {current}/{max}";
    }

    /// <summary>
    /// Gọi khi nhận damage hoặc heal (từ EnemyCombat, item, ...)
    /// </summary>
    public void ChangeHealth(int amount)
    {
        StatsManager.Instance.ChangeHealth(amount);
    }
}
