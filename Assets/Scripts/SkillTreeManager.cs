using TMPro;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TMP_Text pointsText;
    public int availablePoints;
    public void Start()
    {
        foreach (SkillSlot slot in skillSlots)

        {
            slot.skillButton.onClick.AddListener(slot.TryUpgradeSkill);
        }
        UpdateAbilityPoints(0);
    }
    public void UpdateAbilityPoints(int amount )
    {
        availablePoints += amount;
        pointsText.text = "Point: " + availablePoints; 

    }
}
