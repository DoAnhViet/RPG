using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SaveSlotUI : MonoBehaviour
{
    public int slotIndex;
    public TMP_Text timeText, detailText;
    public Button button;

    public void Initialize(int index, SaveData data, Action<int> onClick)
    {
        slotIndex = index;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick(slotIndex));

        if (data != null)
        {
            DateTime dt = DateTime.Parse(data.saveTime);
            timeText.text = dt.ToString("yyyy-MM-dd HH:mm:ss");
            detailText.text = $"Scene {data.sceneIndex}, Playtime {data.playTime:F0}s";
        }
        else
        {
            timeText.text = "— Empty —";
            detailText.text = "";
        }
    }

}
