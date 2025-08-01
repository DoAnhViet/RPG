using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int expReward = 3;
    public delegate void MonsterDefeated(int exp);
    public static event MonsterDefeated OnMonsterDefeated;
    public int currentHealth;
    public int maxHealth;             
    public static Enemy_Health lastDefeated;  // con quái vừa chết
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth (int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if(currentHealth <=0)
        {
            Destroy(gameObject);
            OnMonsterDefeated(expReward);
            lastDefeated = this;             // nhớ con này
        }
    }
}
