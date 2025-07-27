using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ExpManager : MonoBehaviour
{
    public int level;
    public int currentExp;
    public int expToLevel = 10;
    public float expGrowthMultiplier = 1.2f;
    public Slider expSlider;
    public TMP_Text currentLevelText;
    public static event Action<int> OnLevelUp;

    public void Start()
    {
        UpdateUI();
    }
    void Awake()
    {
        var d = SaveLoadManager.pendingLoadedData;
        if (d != null)
        {
            level = d.level;
            currentExp = d.currentExp;
            expToLevel = d.expToLevel;
            // (Có thể thêm expGrowthMultiplier nếu bạn lưu trong SaveData)
            SaveLoadManager.pendingLoadedData = null;
            Debug.Log("[ExpManager] Đã load lại state từ SaveData!");
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GainExperience(2);
        }
    }

    private void OnEnable()
    {
        Enemy_Health.OnMonsterDefeated += GainExperience;
    }
    private void OnDisable()
    {
        Enemy_Health.OnMonsterDefeated -= GainExperience;
    }
    public void GainExperience(int amount)
    {
        currentExp += amount;
        if (currentExp >= expToLevel)
        {
            LevelUp();
        }
        UpdateUI();
    }
    private void LevelUp()
    {
        level++;
        currentExp -= expToLevel;
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Animator anim = player.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("LevelUp");
            }
        }
        expToLevel = Mathf.RoundToInt(expToLevel * expGrowthMultiplier);
        if (level % 5 == 0)
        {
            StatsManager.Instance.damage += 1;
        }
        StatsManager.Instance.maxHealth += 3;
        StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;
        OnLevelUp?.Invoke(1);
    }
    public void UpdateUI()
    {
        expSlider.maxValue = expToLevel;
        expSlider.value = currentExp;
        currentLevelText.text = "Level: " + level;
    }
}
