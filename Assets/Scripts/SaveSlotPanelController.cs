using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SaveSlotsPanelController : MonoBehaviour
{
    public enum Mode { Save, Load }
    public Mode panelMode = Mode.Save;

    public SaveSlotUI[] slotUIs;  // gán 3 Slot prefab instances
    public Button backButton;

    void Awake()
    {
        backButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    void OnEnable()
    {
        RefreshSlots();
    }

    public void RefreshSlots()
    {
        // Load metadata cho cả 3 slot
        List<SaveData> all = SaveLoadManager.LoadAllSlots();
        for (int i = 0; i < slotUIs.Length; i++)
        {
            // truyền callback khác nhau tuỳ mode
            Action<int> onClick = panelMode == Mode.Save
                ? new Action<int>(SaveToSlot)
                : new Action<int>(LoadFromSlot);

            slotUIs[i].Initialize(i, all[i], onClick);
        }
    }

    void SaveToSlot(int idx)
    {
        SaveLoadManager.SaveToSlot(idx);
        // Quay lại cập nhật UI ngay
        RefreshSlots();
    }

    void LoadFromSlot(int idx)
    {
        SaveLoadManager.LoadSlot(idx);
        // Sau khi load, chuyển sang GameScene (nếu từ MainMenu)
        SceneManager.LoadScene("GameScene");
    }
}
