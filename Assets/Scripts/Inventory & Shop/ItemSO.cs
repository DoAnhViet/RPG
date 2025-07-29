using UnityEngine;
[CreateAssetMenu(fileName ="New Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite icon;

    public bool isGold;

    [Header("Stats")]
    public int speed;
    public int maxHealth;
    public int currentHealth;
    public int damage;

    [Header("For Temrorary item")]
    public float duration;

}
