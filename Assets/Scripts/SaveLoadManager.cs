using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SaveLoadManager
{
    public static SaveData pendingLoadedData = null;
    static string baseDir = @"C:\Users\khang\RPG";

    static string SlotPath(int i)
    {
        // Đảm bảo thư mục tồn tại
        if (!Directory.Exists(baseDir))
            Directory.CreateDirectory(baseDir);
        return Path.Combine(baseDir, $"save_slot{i}.json");
    }

    public static void SaveToSlot(int slot)
    {
        var data = Gather();
        File.WriteAllText(SlotPath(slot), JsonUtility.ToJson(data, true));
        Debug.Log($"[SAVE] slot {slot} → {SlotPath(slot)}");
    }

    public static List<SaveData> LoadAllSlots()
    {
        var list = new List<SaveData>();
        for (int i = 0; i < 3; i++)
        {
            string p = SlotPath(i);
            list.Add(File.Exists(p)
                ? JsonUtility.FromJson<SaveData>(File.ReadAllText(p))
                : null);
        }
        return list;
    }

    // ======= SỬA HÀM NÀY: KHÔNG GÁN TRỰC TIẾP, CHỈ GÁN VÀO pendingLoadedData =======
    public static void LoadSlot(int slot)
    {
        string p = SlotPath(slot);
        if (!File.Exists(p))
        {
            Debug.LogWarning($"[LOAD] Không tìm thấy file save slot {slot}!");
            return;
        }
        var d = JsonUtility.FromJson<SaveData>(File.ReadAllText(p));
        pendingLoadedData = d; // <--- chỉ lưu vào biến static này
        Debug.Log($"[LOAD] slot {slot} -> pendingLoadedData đã set, sẽ apply khi scene mới load!");
    }
    // ======= END SỬA HÀM =======

    // ---------------- Ghi lại dữ liệu từ các hệ thống ----------------

    static SaveData Gather()
    {
        var d = new SaveData();

        // 1. Stats
        var stats = StatsManager.Instance;
        d.damage = stats.damage;
        d.speed = stats.speed;
        d.maxHealth = stats.maxHealth;
        d.currentHealth = stats.currentHealth;

        // 2. Exp
        var exp = GameObject.FindObjectOfType<ExpManager>();
        if (exp != null)
        {
            d.level = exp.level;
            d.currentExp = exp.currentExp;
            d.expToLevel = exp.expToLevel;
        }

        // 3. Vị trí player
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var pos = player.transform.position;
            d.playerPosX = pos.x;
            d.playerPosY = pos.y;
            d.playerPosZ = pos.z;
        }

        // 4. Wave & kills
        var wave = GameObject.FindObjectOfType<EnemyWaveManager>();
        if (wave != null)
        {
            d.currentWave = wave.GetCurrentWave();
            d.totalKills = wave.GetTotalKills();
        }

        // 5. Metadata
        d.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        d.saveTime = DateTime.Now.ToString("o");
        d.playTime = Time.timeSinceLevelLoad;

        return d;
    }
}
