using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int damage, speed, maxHealth, currentHealth;
    public int level, currentExp, expToLevel;
    public int availablePoints;
    public List<SkillSlotData> skills;
    public float playerPosX, playerPosY, playerPosZ;
    public int sceneIndex;
    public int currentWave, totalKills;
    public string saveTime;       // ISO string
    public float playTime;        // tổng thời gian chơi
}

[Serializable]
public class SkillSlotData
{
    public string skillID;
    public int currentLevel;
    public bool isUnlocked;
}
