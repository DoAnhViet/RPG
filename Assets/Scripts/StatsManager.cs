using System.Collections;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [Header("Character Selection")]
    public string selectedCharacter = "Human";
    [Header("Combat Stats")]
    public int damage;
    public float weaponRange = 1f;

    [Header("Movement Stats")]
    public int speed;

    [Header("Health Stats")]
    public int maxHealth;
    public int currentHealth;

    [Header("Regen Settings")]
    public float regenDelay = 8f;      // Delay sau khi nhận damage
    public float regenInterval = 5f;   // Mỗi 2s sẽ kiểm tra regen

    private float regenTimer = 0f;


    // Event để thông báo khi HP thay đổi
    public delegate void HealthChanged(int current, int max);
    public static event HealthChanged OnHealthChanged;
    public void UpdateMaxHealth(int amount)
    {
        StatsManager.Instance.maxHealth += amount;
    }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Áp dụng dữ liệu load nếu có
        var d = SaveLoadManager.pendingLoadedData;
        if (d != null)
        {
            damage = d.damage;
            speed = d.speed;
            maxHealth = d.maxHealth;
            currentHealth = d.currentHealth;
            // (Có thể thêm selectedCharacter, weaponRange nếu bạn lưu trong SaveData)
            SaveLoadManager.pendingLoadedData = null; // Clear sau khi dùng
            Debug.Log("[StatsManager] Đã load lại state từ SaveData!");
        }
        else
        {
            // Nếu game mới: giữ logic khởi tạo mặc định như cũ
            currentHealth = maxHealth;
            if (PlayerPrefs.HasKey("SelectedCharacter"))
                selectedCharacter = PlayerPrefs.GetString("SelectedCharacter");
        }
    }
    void Start()
    {
        // Bắt đầu coroutine tự regen
        StartCoroutine(AutoRegenRoutine());
        // Gửi cập nhật UI lần đầu
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        ApplyCharacterBuff(selectedCharacter);
    }

    void Update()
    {
        // Tăng timer kể từ lần chịu damage/heal
        if (regenTimer < regenDelay)
            regenTimer += Time.deltaTime;
    }

    IEnumerator AutoRegenRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenInterval);

            if (regenTimer >= regenDelay && currentHealth > 0)
            {
                int healAmount = Mathf.CeilToInt(maxHealth * 0.05f);
                currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
                OnHealthChanged?.Invoke(currentHealth, maxHealth);
            }
        }
    }

    /// <summary>
    /// Thay đổi HP (damage nếu amount<0, heal nếu >0)
    /// </summary>
    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Reset delay khi nhận damage/heal bất chợt
        regenTimer = 0f;

        // Thông báo UI
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // Nếu chết thì game over
        if (currentHealth <= 0)
            GameManager.Instance?.GameOver();
    }
    // Các method buff
    public void BuffMaxHealth(int amount)
    {
        StatsManager.Instance.maxHealth += amount;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void BuffDamage(int amount)
    {
        StatsManager.Instance.damage += amount;
    }

    public void BuffSpeedPercent(float percent)
    {
        StatsManager.Instance.speed = Mathf.RoundToInt(speed * (1f + percent));
    }

    public void BuffRegenInterval(float newInterval)
    {
        StatsManager.Instance.regenInterval = newInterval;
    }

    /// <summary>
    /// Áp buff theo character name
    /// </summary>
    public void ApplyCharacterBuff(string character)
    {
        selectedCharacter = character;
        PlayerPrefs.SetString("SelectedCharacter", character);
        PlayerPrefs.Save();
        switch (character)
        {
            case "Elf":
                BuffDamage(1);
                break;
            case "Dwarf":
                BuffMaxHealth(10);
                break;
            case "Demon":
                BuffRegenInterval(2f);
                BuffDamage(2);
                break;
            case "Angel":
                BuffDamage(1);
                BuffSpeedPercent(0.2f);
                break;
            default:
                // Human, Skeleton: no buff
                break;
        }
    }
}
