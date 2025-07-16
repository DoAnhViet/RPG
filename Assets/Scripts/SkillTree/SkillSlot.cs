using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SkillSlot : MonoBehaviour
{
    public SkillS0 skillS0;
    public bool isUnlocked;
    public Image skillIcon;
    public int currentLevel;
    public Button skillButton;
    public TMP_Text skillLevelText;
    private void OnValidate()
    {
        if (skillS0 != null && skillLevelText != null )
        {
            UpdateUI();
        }
    }
    public void TryUpgradeSkill()
    {
        if (isUnlocked && currentLevel < skillS0.maxLevel)
        {
            currentLevel++;
            UpdateUI();
        }    
    }
    public void UpdateUI()
    {
        skillIcon.sprite = skillS0.skillIcon;
        if(isUnlocked)
        {
            skillButton.interactable = true;
            skillLevelText.text = currentLevel.ToString() + "/" + skillS0.maxLevel.ToString();
            skillIcon.color = Color.white;
        }
        else
        {
            skillButton.interactable = false;
            skillLevelText.text = "Locked";
            skillIcon.color = Color.grey;
        }
    }

}
