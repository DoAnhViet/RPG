using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;
public class SkillSlot : MonoBehaviour
{
    public List<SkillSlot> prerequisiteSkillSlots; 
    public SkillS0 skillS0;
    public bool isUnlocked;
    public Image skillIcon;
    public int currentLevel;
    public Button skillButton;
    public TMP_Text skillLevelText;
    public static event Action<SkillSlot> OnAbilityPointSpent;
    public static event Action<SkillSlot> OnSkillMaxed;
    private void OnValidate()
    {
        if (skillS0 != null && skillLevelText != null )
        {
            UpdateUI();
        }
    }
    public void TryUpgradeSkill()
    {
        if (isUnlocked == true && currentLevel < skillS0.maxLevel)
        {
            currentLevel++;
            OnAbilityPointSpent?.Invoke(this);
            if (currentLevel >= skillS0.maxLevel)
            {
                OnSkillMaxed?.Invoke(this);
            }
            UpdateUI();  
        }    
    }
    public bool CanUnlockSkill()
    {
        foreach (SkillSlot slot in prerequisiteSkillSlots)
        {
            if(!slot.isUnlocked || slot.currentLevel < slot.skillS0.maxLevel)
            {
                return false;
            }
        }
        return true;
    }
    public void UpdateUI()
    {
        skillIcon.sprite = skillS0.skillIcon;
        if(isUnlocked == true)
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
    public void Unlock()
    {
        isUnlocked = true;
        UpdateUI();

    }
}
