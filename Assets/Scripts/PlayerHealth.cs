using UnityEngine;
using TMPro;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    private float regenDelay = 5f; // Delay sau khi nhận damage
    private float regenDelayTimer = 0f;
    public TMP_Text healthText;

    private void Start()
    {
        healthText.text = "HP: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth;
        StartCoroutine(AutoRegenHealth());
    }
    void Update()
    {
        // Đếm thời gian kể từ lần nhận damage gần nhất
        if (regenDelayTimer < regenDelay)
        {
            regenDelayTimer += Time.deltaTime;
        }
    }

    IEnumerator AutoRegenHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // Mỗi 2s kiểm tra

            // Chỉ hồi máu nếu đã đủ delay không bị đánh (regenDelayTimer >= regenDelay)
            if (regenDelayTimer >= regenDelay)
            {
                int healAmount = Mathf.CeilToInt(StatsManager.Instance.maxHealth * 0.05f);
                if (StatsManager.Instance.currentHealth < StatsManager.Instance.maxHealth)
                {
                    StatsManager.Instance.currentHealth += healAmount;
                    if (StatsManager.Instance.currentHealth > StatsManager.Instance.maxHealth)
                        StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;

                    healthText.text = "HP: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth;
                }
            }
            // Nếu chưa đủ delay thì không làm gì, chờ lần check sau
        }
    }
    public void ChangeHealth(int amount)
    {
        StatsManager.Instance.currentHealth += amount;
        if (StatsManager.Instance.currentHealth > StatsManager.Instance.maxHealth)
            StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;

        healthText.text = "HP: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth;

        // Nếu nhận damage (amount < 0), reset timer delay hồi máu
        if (amount < 0)
        {
            regenDelayTimer = 0f;
        }

        if (StatsManager.Instance.currentHealth <= 0)
        {
            gameObject.SetActive(false);
        } 
    }
}
