using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SpinManager : MonoBehaviour
{
    [Header("UI")]
    public Button spinButton;
    public TMP_Text resultText;
    public TMP_Text pityText;
    public TMP_Text spinsLeftText;    // Text hiển số lượt còn lại

    [Header("Spin Settings")]
    public int maxSpins = 10;         // Số lượt spin tối đa

    private int spinsUsed = 0;        // Đã dùng bao nhiêu lượt spin
    private int spinCount = 0;        // Đếm spin liên tiếp không ra Rare/Legendary
    private const int pityThreshold = 20;

    private float commonRate = 0.70f;
    private float rareRate = 0.27f;
    private float legendRate = 0.03f;
    public static SpinManager Instance;
    public GameObject spinPopupPrefab;
    public Canvas uiCanvas;


    private enum Tier { Common, Rare, Legendary }
    private Dictionary<Tier, string[]> tierPools = new Dictionary<Tier, string[]>()
    {
        { Tier.Common,    new[]{ "Human", "Skeleton" } },
        { Tier.Rare,      new[]{ "Elf", "Dwarf" } },
        { Tier.Legendary, new[]{ "Demon", "Angel" } }
    };
    // …

    public void IncrementSpin()
    {
        if (spinsUsed >= maxSpins) return;
        spinsUsed++;
        UpdateSpinsLeftUI();
    }
    void Start()
    {
        spinButton.onClick.AddListener(DoSpin);
        UpdateSpinsLeftUI();
    }

    /// <summary>
    /// Gọi khi nhấn nút Spin
    /// </summary>
    public void DoSpin()
    {
        if (spinsUsed >= maxSpins) return;

        spinsUsed++;
        spinCount++;

        Tier tier = DetermineTier();
        string character = PickFromPool(tier);
        ApplyBuff(character);

        resultText.text = $"You got: <b>{character}</b> ({tier})";
        pityText.text = $"Pity: {spinCount}/{pityThreshold}";

        UpdateSpinsLeftUI();

        if (tier != Tier.Common)
            spinCount = 0;  // reset pity khi ra Rare/Legendary
        PlayerPrefs.SetString("SpinCharacter", character);
        PlayerPrefs.Save();
        ApplyBuff(character);
    }

    private void UpdateSpinsLeftUI()
    {
        int spinsLeft = Mathf.Max(0, maxSpins - spinsUsed);
        if (spinsLeftText != null)
            spinsLeftText.text = $"Spins Left: {spinsLeft}/{maxSpins}";
        spinButton.interactable = spinsLeft > 0;
    }

    private Tier DetermineTier()
    {
        if (spinCount >= pityThreshold)
            return Tier.Rare;

        float r = Random.value;
        if (r < legendRate) return Tier.Legendary;
        if (r < legendRate + rareRate) return Tier.Rare;
        return Tier.Common;
    }

    private string PickFromPool(Tier tier)
    {
        var pool = tierPools[tier];
        return pool[Random.Range(0, pool.Length)];
    }

    private void ApplyBuff(string character)
    {
        StatsManager.Instance.ApplyCharacterBuff(character);

        // Nếu bạn muốn refresh UI ngay:
        var ui = Object.FindFirstObjectByType<StatsUI>();
        if (ui != null) ui.UpdateAllStats();
    }
}
