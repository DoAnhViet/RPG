using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;
    public CanvasGroup statsCanvas;
    private bool statsOpen = false;
    private void Start()
    {
        UpdateAllStats();
    }
    private void Update()
    {
        if (Input.GetButtonDown("ToggleStats"))

            if (statsOpen)
            {
                Time.timeScale = 1;
                statsCanvas.alpha = 0;
                statsOpen = false;
            }
            else
            {
                Time.timeScale = 0;
                statsCanvas.alpha = 1;
                statsOpen = true;
            }




    }
    public void UpdateDamage()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.damage;
    }
    public void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.speed;
    }
    public void UpdateHealth()
    {
        statsSlots[2].GetComponentInChildren<TMP_Text>()
            .text = "Max HP: " + StatsManager.Instance.maxHealth;
    }

    public void UpdateRegen()
    {
        statsSlots[3].GetComponentInChildren<TMP_Text>()
            .text = $"Regen: 1 per {StatsManager.Instance.regenInterval}s";
    }

    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
        UpdateHealth();
        UpdateRegen();
    }
}
